Imports AutoMapper
Imports S4E.Context.Dtos.AssociadoDto
Imports S4E.Models

Namespace Profiles
    Public Class AssociadoProfile
        Inherits Profile
        Public Sub New()
            CreateMap(Of CreateAssociadoDto, Associado)()
            CreateMap(Of Associado, ReadAssociadoDto)()
            CreateMap(Of UpdateAssociadoDto, Associado)()
            CreateMap(Of Associado, GetAssociadoDto)()
        End Sub
    End Class
End Namespace

