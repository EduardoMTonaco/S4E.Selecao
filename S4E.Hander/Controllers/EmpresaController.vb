Imports AutoMapper
Imports Microsoft.AspNetCore.Mvc
Imports S4E.Context
Imports S4E.Context.Dtos.EmpresaDto
Imports S4E.Models
Imports S4E.Service
Imports FluentResults
Imports S4E.Context.Dtos

Namespace Controllers
    <ApiController>
    <Route("[controller]")>
    Public Class EmpresaController


#Region "PROPRIEDADES"
        Private Property _empresaService As EmpresaService

#End Region

#Region "CONSTRUTORES"
        Public Sub New(empresaService As EmpresaService)
            _empresaService = empresaService
        End Sub

#End Region
#Region "MÉTODOS"
        <HttpPost>
        Public Function AdicionaEmpresa(<FromBody> empresaDto As CreateEmpresaDto) As IActionResult
            Dim empresa As GetEmpresaDto = _empresaService.AdicionaEmpresa(empresaDto)

            Return New CreatedAtActionResult(NameOf(RecuperaEmpresaPorId), "empresa",
                                             New With {Key empresa.Id}, empresa)
        End Function
        <HttpGet>
        Public Function RecuperaEmpresas() As IEnumerable(Of GetEmpresaDto)
            Return _empresaService.RecuperaEmpresas()
        End Function

        <HttpGet("nome/{nome}")>
        Public Function RecuperaEmpresasPorNome(nome As String) As IEnumerable(Of GetEmpresaDto)
            Return _empresaService.RecuperaAssociadosPorNome(nome)
        End Function

        <HttpGet("id/{id}")>
        Public Function RecuperaEmpresaPorId(id As Integer) As IActionResult
            Dim empresaDto As GetEmpresaDto = _empresaService.RecuperaEmpresaPorId(id)
            If empresaDto IsNot Nothing Then
                Return New OkObjectResult(empresaDto)
            End If
            Return New NotFoundObjectResult(empresaDto)
        End Function

        <HttpGet("cnpj/{cnpj}")>
        Public Function RecuperaEmpresaPorCNPJ(cnpj As String) As IActionResult
            Dim empresaDto As GetEmpresaDto = _empresaService.RecuperaEmpresaPorCNPJ(cnpj)
            If empresaDto IsNot Nothing Then
                Return New OkObjectResult(empresaDto)
            End If
            Return New NotFoundObjectResult(empresaDto)
        End Function
        <HttpPost("{id}")>
        Public Function AtualizaEmpresa(id As Integer, <FromBody> empresaDto As UpdateEmpresaDto) As IActionResult
            Dim resultado As Result = _empresaService.AtualizaEmpresa(id, empresaDto)
            If resultado.IsFailed Then
                Return New NotFoundResult
            End If
            Return New NoContentResult
        End Function

        <HttpDelete("{id}")>
        Public Function DeletaEmpresa(id As Integer) As IActionResult
            Dim resultado As Result = _empresaService.DeletaEmpresa(id)
            If resultado.IsFailed Then
                Return New NotFoundResult
            End If
            Return New NoContentResult
        End Function

#End Region
    End Class

End Namespace