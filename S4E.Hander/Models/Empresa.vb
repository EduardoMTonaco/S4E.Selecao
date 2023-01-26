Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Microsoft.EntityFrameworkCore

Namespace Models
    Public Class Empresa

#Region "Prpriedades"
        <Required>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id As Integer

        <MaxLength(200, ErrorMessage:="O nome não pode ultrapassar 200 caracteres")>
        <Required(ErrorMessage:="Nome Necessario")>
        Public Property Nome As String


        <Required(ErrorMessage:="CNPJ Necessario")>
        <StringLength(14, ErrorMessage:="CNPJ deve ter 14 caracteres numericos")>
        Public Property Cnpj As String
        Public Overridable Property AssociadosEmpresas As ICollection(Of AssociadoEmpresa)
#End Region
#Region "Consturores"
        Public Sub New()
            AssociadosEmpresas = New HashSet(Of AssociadoEmpresa)
        End Sub
#End Region
#Region "Métodos"

#End Region
    End Class
End Namespace

