Imports System.ComponentModel.DataAnnotations
Imports S4E.Models

Namespace Context.Dtos.AssociadoDto
    Public Class UpdateAssociadoDto

        Public Property Nome As String

        <StringLength(11, ErrorMessage:="CPF deve ter 11 caracteres numericos")>
        Public Property Cpf As String
        Public Property EmpresasId As ICollection(Of Integer)
        Public Sub New()
            EmpresasId = New HashSet(Of Integer)
        End Sub

    End Class
End Namespace

