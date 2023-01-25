Imports S4E.Context.Dtos.EmpresaDto
Imports S4E.Models

Namespace Context.Dtos.AssociadoDto
    Public Class GetAssociadoDto
        Public Property Id As Integer
        Public Property Nome As String
        Public Property Cpf As String
        Public Property Empresas As ICollection(Of ReadEmpresaDto)
        Public Sub New()
            Empresas = New HashSet(Of ReadEmpresaDto)
        End Sub
    End Class
End Namespace

