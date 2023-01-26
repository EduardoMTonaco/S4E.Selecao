Imports AutoMapper
Imports FluentResults
Imports Microsoft.Build.Experimental.ProjectCache
Imports S4E.Context
Imports S4E.Context.Dtos.AssociadoDto
Imports S4E.Context.Dtos.EmpresaDto
Imports S4E.Models

Namespace Service
    Public Class EmpresaService


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
        Public Function AdicionaEmpresa(empresaDto As CreateEmpresaDto) As GetEmpresaDto
            Dim empresa As Empresa = _mapper.Map(Of Empresa)(empresaDto)
            _context.Empresas.Add(empresa)
            _context.SaveChanges()
            Dim empresaMap As GetEmpresaDto = _mapper.Map(Of GetEmpresaDto)(empresa)
            For Each i In empresaDto.AssociadosId
                Dim associadoEmpresas As New AssociadoEmpresa()
                associadoEmpresas.EmpresaId = empresa.Id
                associadoEmpresas.EmpresaId = i
                _context.AssociadoEmpresa.Add(associadoEmpresas)
            Next
            _context.SaveChanges()
            Return empresaMap
        End Function

        Public Function RecuperaEmpresas() As IEnumerable(Of GetEmpresaDto)
            Dim empresas As New List(Of Empresa)
            For Each empresa In _context.Empresas
                empresas.Add(empresa)
            Next
            Dim readempresas As New List(Of GetEmpresaDto)
            For Each empresa In empresas
                Dim readempresa As GetEmpresaDto = PreencheEmpresaAssociado(empresa)
                readempresas.Add(readempresa)
            Next

            Return readempresas
        End Function

        Public Function RecuperaEmpresaPorId(id As Integer) As GetEmpresaDto
            Dim empresa As Empresa = _context.Empresas.FirstOrDefault(Function(a) a.Id = id)

            If empresa IsNot Nothing Then
                Return PreencheEmpresaAssociado(empresa)
            End If
            Return Nothing
        End Function

        Public Function RecuperaEmpresaPorCNPJ(cnpj As String) As GetEmpresaDto
            Dim empresa As Empresa = _context.Empresas.FirstOrDefault(Function(a) a.Cnpj = cnpj)

            If empresa IsNot Nothing Then
                Return PreencheEmpresaAssociado(empresa)
            End If
            Return Nothing
        End Function
        Public Function RecuperaAssociadosPorNome(nome As String) As IEnumerable(Of GetEmpresaDto)
            Dim empresas As New List(Of GetEmpresaDto)
            Dim nomeReplace As String = nome.Replace("-", " ")
            Dim empresaLista As New List(Of Empresa)
            For Each empresa In _context.Empresas
                If empresa.Nome = nomeReplace Then
                    empresaLista.Add(empresa)
                End If
            Next
            For Each empresa In empresaLista
                If empresa.Nome = nomeReplace Then
                    empresas.Add(PreencheEmpresaAssociado(empresa))
                End If
            Next
            Return empresas
        End Function

        Public Function AtualizaEmpresa(id As Integer, empresaDto As UpdateEmpresaDto) As Result
            Dim empresa As Empresa = _context.Empresas.FirstOrDefault(Function(a) a.Id = id)
            If empresa Is Nothing Then
                Return Result.Fail("empresa não encontrada")
            End If
            For Each ae In _context.AssociadoEmpresa
                If ae.EmpresaId = empresa.Id Then
                    _context.Remove(ae)
                End If
            Next
            For Each i In empresaDto.AssociadosId
                Dim empresasEmpresas As New AssociadoEmpresa()
                empresasEmpresas.EmpresaId = empresa.Id
                empresasEmpresas.AssociadoId = i
                _context.AssociadoEmpresa.Add(empresasEmpresas)
            Next
            If String.IsNullOrEmpty(empresaDto.Cnpj) Then
                empresaDto.Cnpj = empresa.Cnpj
            End If
            If String.IsNullOrEmpty(empresaDto.Nome) Then
                empresaDto.Nome = empresa.Nome
            End If

            _mapper.Map(empresaDto, empresa)
            _context.SaveChanges()
            Return Result.Ok()
        End Function

        Public Function DeletaEmpresa(id As Integer)
            Dim empresa As Empresa = _context.Empresas.FirstOrDefault(Function(a) a.Id = id)
            If empresa Is Nothing Then
                Return Result.Fail("empresa não encontrada")
            End If
            _context.Remove(empresa)
            For Each ae In _context.AssociadoEmpresa
                If ae.EmpresaId = empresa.Id Then
                    _context.Remove(ae)
                End If
            Next

            _context.SaveChanges()
            Return Result.Ok()
        End Function

#End Region

#Region "MÉTODOS PRIVADOS"
        Private Function PreencheEmpresaAssociado(empresa As Empresa) As GetEmpresaDto
            Dim empresaDto As GetEmpresaDto = _mapper.Map(Of GetEmpresaDto)(empresa)
            Dim AssociadoIds As New HashSet(Of Integer)
            For Each ae In _context.AssociadoEmpresa
                If ae.EmpresaId = empresa.Id Then
                    AssociadoIds.Add(ae.AssociadoId)
                End If
            Next
            For Each i In AssociadoIds
                Dim associado As Associado = _context.Associados.FirstOrDefault(Function(e) e.Id = i)
                Dim readAssociadoDto As ReadAssociadoDto = _mapper.Map(Of ReadAssociadoDto)(associado)
                empresaDto.Associados.Add(readAssociadoDto)
            Next
            Return empresaDto
        End Function

        Private Function PreencheEmpresaAssociado(empresaDto As GetEmpresaDto) As GetEmpresaDto
            Dim AssociadoIds As New HashSet(Of Integer)
            For Each ae In _context.AssociadoEmpresa
                If ae.EmpresaId = empresaDto.Id Then
                    AssociadoIds.Add(ae.AssociadoId)
                End If
            Next
            For Each i In AssociadoIds
                Dim associado As Associado = _context.Associados.FirstOrDefault(Function(e) e.Id = i)
                Dim readAssociadoDto As ReadAssociadoDto = _mapper.Map(Of ReadAssociadoDto)(associado)
                empresaDto.Associados.Add(readAssociadoDto)
            Next
            Return empresaDto
        End Function
#End Region

    End Class
End Namespace
