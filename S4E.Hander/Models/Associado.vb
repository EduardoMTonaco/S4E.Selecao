Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Security.Policy
Imports Microsoft.EntityFrameworkCore

Namespace Models
    Public Class Associado
#Region "Prpriedades"
        <Required>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id As Integer

        <Required(ErrorMessage:="Nome Necessario")>
        <MaxLength(200, ErrorMessage:="O nome não pode ultrapassar 200 caracteres")>
        Public Property Nome As String

        <Required(ErrorMessage:="Cpf Necessario")>
        <StringLength(11, ErrorMessage:="CPF deve ter 11 caracteres numericos")>
        Public Property Cpf As String

        Public Property DataDeNascimento As DateTime?
        Public Property EmpresasId As ICollection(Of Integer)
        Public Overridable Property AssociadosEmpresas As ICollection(Of AssociadosEmpresas)


#End Region
#Region "Consturores"
        Public Sub New()
            AssociadosEmpresas = New HashSet(Of AssociadosEmpresas)
            EmpresasId = New HashSet(Of Integer)
        End Sub
#End Region
#Region "Metodos"

#End Region

    End Class
End Namespace

