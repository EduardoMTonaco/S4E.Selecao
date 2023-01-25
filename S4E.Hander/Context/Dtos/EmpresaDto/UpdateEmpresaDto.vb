Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Internal
Imports S4E.Models
Imports System.ComponentModel.DataAnnotations

Namespace Context.Dtos.EmpresaDto
    Public Class UpdateEmpresaDto
        Public Property Nome As String
        <Required>
        <StringLength(14, ErrorMessage:="CNPJ deve ter 14 caracteres numericos")>
        Public Property Cnpj As String
        Public Property AssociadosId As ICollection(Of Integer)

        Public Sub New()
            AssociadosId = New HashSet(Of Integer)
        End Sub
    End Class
End Namespace

