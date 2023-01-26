Imports AutoMapper
Imports Microsoft.AspNetCore.Mvc
Imports S4E.Context
Imports S4E.Context.Dtos.AssociadoDto
Imports S4E.Models
Imports S4E.Service
Imports FluentResults
Imports S4E.Context.Dtos

Namespace Controllers
    <ApiController>
    <Route("[controller]")>
    Public Class AssociadoController


#Region "PROPRIEDADES"
        Private Property _associadoService As AssociadoService

#End Region

#Region "CONSTRUTORES"
        Public Sub New(associadoService As AssociadoService)
            _associadoService = associadoService
        End Sub

#End Region
#Region "MÉTODOS"
        <HttpPost>
        Public Function AdicionaAssociado(<FromBody> associadoDto As CreateAssociadoDto) As IActionResult
            Dim associado As GetAssociadoDto = _associadoService.AdicionaAssociado(associadoDto)

            Return New CreatedAtActionResult(NameOf(RecuperaAssociadoPorId), "Associado",
                                             New With {Key associado.Id}, associado)
        End Function
        <HttpGet>
        Public Function RecuperaAssociados() As IEnumerable(Of GetAssociadoDto)
            Return _associadoService.RecuperaAssociados()
        End Function
        <HttpGet("nome/{nome}")>
        Public Function RecuperaAssociadosPorNome(nome As String) As IEnumerable(Of GetAssociadoDto)
            Return _associadoService.RecuperaAssociadosPorNome(nome)
        End Function

        <HttpGet("id/{id}")>
        Public Function RecuperaAssociadoPorId(id As Integer) As IActionResult
            Dim associadoDto As GetAssociadoDto = _associadoService.RecuperaAssociadoPorId(id)
            If associadoDto IsNot Nothing Then
                Return New OkObjectResult(associadoDto)

            End If
            Return New NotFoundObjectResult(associadoDto)
        End Function

        <HttpGet("cpf/{cpf}")>
        Public Function RecuperaAssociadoPorCPF(cpf As String) As IActionResult
            Dim associadoDto As GetAssociadoDto = _associadoService.RecuperaAssociadoPorCPF(cpf)
            If associadoDto IsNot Nothing Then
                Return New OkObjectResult(associadoDto)

            End If
            Return New NotFoundObjectResult(associadoDto)
        End Function

        <HttpPost("{id}")>
        Public Function AtualizaAssociado(id As Integer, <FromBody> associadoDto As UpdateAssociadoDto) As IActionResult
            Dim resultado As Result = _associadoService.AtualizaAssociado(id, associadoDto)
            If resultado.IsFailed Then
                Return New NotFoundResult
            End If
            Return New NoContentResult
        End Function

        <HttpDelete("{id}")>
        Public Function DeletaAssociado(id As Integer) As IActionResult
            Dim resultado As Result = _associadoService.DeletaAssociado(id)
            If resultado.IsFailed Then
                Return New NotFoundResult
            End If
            Return New NoContentResult
        End Function

#End Region
    End Class

End Namespace