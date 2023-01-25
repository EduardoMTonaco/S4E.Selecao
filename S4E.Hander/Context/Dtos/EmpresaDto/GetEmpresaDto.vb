Imports S4E.Context.Dtos.AssociadoDto
Imports S4E.Models
Imports System.ComponentModel.DataAnnotations

Namespace Context.Dtos.EmpresaDto
    Public Class GetEmpresaDto
        Public Property Id As Integer

        'Nome( varchar, 200, notnull
        <Required(ErrorMessage:="Nome Necessario")>
        Public Property Nome As String

        'Cpf(vcchar, 11, notnull
        <Required(ErrorMessage:="CNPJ Necessario")>
        <StringLength(14, ErrorMessage:="CNPJ deve ter 14 caracteres numericos")>
        Public Property Cnpj As String
        Public Property Associados As ICollection(Of ReadAssociadoDto)

        Public Sub New()
            Associados = New HashSet(Of ReadAssociadoDto)
        End Sub
    End Class
End Namespace

