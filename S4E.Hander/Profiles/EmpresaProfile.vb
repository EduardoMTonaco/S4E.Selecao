Imports AutoMapper
Imports S4E.Context.Dtos.EmpresaDto
Imports S4E.Models

Namespace Profiles
    Public Class EmpresaProfile
        Inherits Profile
        Public Sub New()
            CreateMap(Of CreateEmpresaDto, Empresa)()
            CreateMap(Of Empresa, ReadEmpresaDto)()
            CreateMap(Of UpdateEmpresaDto, Empresa)()
            CreateMap(Of Empresa, GetEmpresaDto)()
        End Sub
    End Class

End Namespace
