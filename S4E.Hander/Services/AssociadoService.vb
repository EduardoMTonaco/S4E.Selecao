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
        Public Function AdicionaAssociado(associadoDto As CreateAssociadoDto) As GetAssociadoDto
            Dim associado As Associado = _mapper.Map(Of Associado)(associadoDto)
            _context.Associados.Add(associado)
            _context.SaveChanges()
            Dim getAssociadoDto As GetAssociadoDto = _mapper.Map(Of GetAssociadoDto)(associado)
            For Each i In associadoDto.EmpresasId
                Dim associadosEmpresas As New AssociadoEmpresa()
                associadosEmpresas.AssociadoId = associado.Id
                associadosEmpresas.EmpresaId = i
                _context.AssociadoEmpresa.Add(associadosEmpresas)
                _context.SaveChanges()
            Next

            getAssociadoDto = PreencheEmpresasAssociado(associado)
            Return getAssociadoDto
        End Function

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
        Public Function RecuperaAssociadosPorNome(nome As String) As IEnumerable(Of GetAssociadoDto)
            Dim associados As New List(Of GetAssociadoDto)
            Dim nomeReplace As String = nome.Replace("-", " ")
            Dim associadoLista As New List(Of Associado)
            For Each associado In _context.Associados
                If associado.Nome = nomeReplace Then
                    associadoLista.Add(associado)
                End If
            Next
            For Each associado In associadoLista
                If associado.Nome = nomeReplace Then
                    associados.Add(PreencheEmpresasAssociado(associado))
                End If
            Next
            Return associados
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
                Dim associadosEmpresas As New AssociadoEmpresa()
                associadosEmpresas.AssociadoId = associado.Id
                associadosEmpresas.EmpresaId = i
                _context.AssociadoEmpresa.Add(associadosEmpresas)
            Next
            If String.IsNullOrEmpty(associadoDto.Cpf) Then
                associadoDto.Cpf = associado.Cpf
            End If
            If String.IsNullOrEmpty(associadoDto.Nome) Then
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

        Private Function PreencheEmpresaAssociado(associadoDto As GetAssociadoDto) As GetAssociadoDto
            Dim AssociadoIds As New HashSet(Of Integer)
            For Each ae In _context.AssociadoEmpresa
                If ae.AssociadoId = associadoDto.Id Then
                    AssociadoIds.Add(ae.EmpresaId)
                End If
            Next
            For Each i In AssociadoIds
                Dim associado As Associado = _context.Associados.FirstOrDefault(Function(e) e.Id = i)
                Dim readAssociadoDto As ReadEmpresaDto = _mapper.Map(Of ReadEmpresaDto)(associado)
                associadoDto.Empresas.Add(readAssociadoDto)
            Next
            Return associadoDto
        End Function

#End Region

    End Class
End Namespace
