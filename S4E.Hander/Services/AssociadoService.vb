Imports AutoMapper
Imports FluentResults
Imports S4E.Context
Imports S4E.Context.Dtos.AssociadoDto
Imports S4E.Context.Dtos.EmpresaDto
Imports S4E.Models

Namespace Service
    Public Class AssociadoService


#Region "PROPRIEDADES"
        Private Property _context As SQLServerDbContext
        Private Property _mapper As IMapper
#End Region

#Region "CONSTRUTORES"
        Public Sub New(context As SQLServerDbContext, mapper As IMapper)
            _context = context
            _mapper = mapper
        End Sub
#End Region

#Region "MÉTODOS"
        Public Function AdicionaAssociado(associadoDto As CreateAssociadoDto) As ReadAssociadoDto
            Dim associado As Associado = _mapper.Map(Of Associado)(associadoDto)
            _context.Associados.Add(associado)
            _context.SaveChanges()
            Dim associadoMap As ReadAssociadoDto = _mapper.Map(Of ReadAssociadoDto)(associado)
            For Each i In associadoDto.EmpresasId
                Dim associadosEmpresas As New AssociadosEmpresas()
                associadosEmpresas.AssociadoId = associado.Id
                associadosEmpresas.EmpresaId = i
                _context.AssociadoEmpresa.Add(associadosEmpresas)
            Next
            _context.SaveChanges()
            Return associadoMap
        End Function
        '_mapper.Map<ReadFilmeDto>(filme)

        Public Function RecuperaAssociados() As IEnumerable(Of GetAssociadoDto)
            Dim associados As New List(Of Associado)
            For Each associado In _context.Associados
                associados.Add(associado)
            Next
            Dim readAssociados As New List(Of GetAssociadoDto)
            For Each associado In associados
                Dim readAssociado As GetAssociadoDto = PreencheEmpresasAssociado(associado)
                readAssociados.Add(readAssociado)
            Next

            Return readAssociados
        End Function

        Public Function RecuperaAssociadoPorId(id As Integer) As GetAssociadoDto
            Dim associado As Associado = _context.Associados.FirstOrDefault(Function(a) a.Id = id)

            If associado IsNot Nothing Then
                Return PreencheEmpresasAssociado(associado)
            End If
            Return Nothing
        End Function
        Public Function RecuperaAssociadoPorCPF(cpf As String) As GetAssociadoDto
            Dim associado As Associado = _context.Associados.FirstOrDefault(Function(a) a.Cpf = cpf)

            If associado IsNot Nothing Then
                Return PreencheEmpresasAssociado(associado)
            End If
            Return Nothing
        End Function

        Public Function AtualizaAssociado(id As Integer, associadoDto As UpdateAssociadoDto) As Result
            Dim associado As Associado = _context.Associados.FirstOrDefault(Function(a) a.Id = id)
            If associado Is Nothing Then
                Return Result.Fail("Associado não encontrada")
            End If
            For Each ae In _context.AssociadoEmpresa
                If ae.AssociadoId = associado.Id Then
                    _context.Remove(ae)
                End If
            Next
            For Each i In associadoDto.EmpresasId
                Dim associadosEmpresas As New AssociadosEmpresas()
                associadosEmpresas.AssociadoId = associado.Id
                associadosEmpresas.EmpresaId = i
                _context.AssociadoEmpresa.Add(associadosEmpresas)
            Next
            If associadoDto.Cpf Is Nothing Then
                associadoDto.Cpf = associado.Cpf
            End If
            If associadoDto.Nome Is Nothing Then
                associadoDto.Nome = associado.Nome
            End If

            _mapper.Map(associadoDto, associado)

            _context.SaveChanges()
            Return Result.Ok()
        End Function

        Public Function DeletaAssociado(id As Integer)
            Dim associado As Associado = _context.Associados.FirstOrDefault(Function(a) a.Id = id)
            If associado Is Nothing Then
                Return Result.Fail("Associado não encontrada")
            End If
            _context.Remove(associado)
            For Each ae In _context.AssociadoEmpresa
                If ae.AssociadoId = associado.Id Then
                    _context.Remove(ae)
                End If
            Next
            _context.SaveChanges()
            Return Result.Ok()
        End Function

#End Region

#Region "MÉTODOS PRIVADOS"
        Private Function PreencheEmpresasAssociado(associado As Associado) As GetAssociadoDto
            Dim associadoDto As GetAssociadoDto = _mapper.Map(Of GetAssociadoDto)(associado)
            Dim empresasId As New HashSet(Of Integer)
            For Each ae In _context.AssociadoEmpresa
                If ae.AssociadoId = associado.Id Then
                    empresasId.Add(ae.EmpresaId)
                End If
            Next
            For Each i In empresasId
                Dim empresa As Empresa = _context.Empresas.FirstOrDefault(Function(e) e.Id = i)
                Dim readEmpresaDto As ReadEmpresaDto = _mapper.Map(Of ReadEmpresaDto)(empresa)
                associadoDto.Empresas.Add(readEmpresaDto)
            Next
            Return associadoDto
        End Function
#End Region

    End Class
End Namespace
