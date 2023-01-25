Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports S4E.Models

Namespace Context.Dtos.AssociadoDto
    Public Class CreateAssociadoDto

        <Required(ErrorMessage:="Nome Necessario")>
        Public Property Nome As String

        <Required(ErrorMessage:="Cpf Necessario")>
        <StringLength(11, ErrorMessage:="CPF deve ter 11 caracteres numericos")>
        Public Property Cpf As String
        Public Property DataDeNascimento As DateTime
        Public Property EmpresasId As ICollection(Of Integer)


        Public Sub New()
            EmpresasId = New HashSet(Of Integer)
        End Sub

    End Class


End Namespace
