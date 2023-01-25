Imports Microsoft.EntityFrameworkCore
Imports S4E.Models

Namespace Context
    Public Class SQLServerDbContext
        Inherits DbContext

#Region "Propriedades"
        Public Property Associados As DbSet(Of Associado)
        Public Property Empresas As DbSet(Of Empresa)
        Public Property AssociadoEmpresa As DbSet(Of AssociadosEmpresas)
#End Region

#Region "Construtores"
        Public Sub New(opt As DbContextOptions(Of SQLServerDbContext))
            MyBase.New(opt)

        End Sub
        Public Sub New()

        End Sub
#End Region

#Region "Métodos"
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)

            modelBuilder.Entity(Of Associado)().Ignore(Function(a) a.EmpresasId)

            modelBuilder.Entity(Of Empresa)().HasKey(Function(e) e.Id)
            modelBuilder.Entity(Of Associado)().HasKey(Function(a) a.Id)

            modelBuilder.Entity(Of Empresa)().HasIndex(Function(e) e.Cnpj).IsUnique()
            modelBuilder.Entity(Of Associado)().HasIndex(Function(a) a.Cpf).IsUnique()

            modelBuilder.Entity(Of AssociadosEmpresas)().HasKey(Function(ae) New With {ae.AssociadoId, ae.EmpresaId})

            modelBuilder.Entity(Of AssociadosEmpresas)() _
                .HasOne(Function(ae) ae.Associado) _
                .WithMany(Function(a) a.AssociadosEmpresas) _
                .HasForeignKey(Function(ae) ae.AssociadoId)

            modelBuilder.Entity(Of AssociadosEmpresas)() _
                .HasOne(Function(ae) ae.Empresa) _
                .WithMany(Function(e) e.AssociadosEmpresas) _
                .HasForeignKey(Function(ae) ae.EmpresaId)
        End Sub

#End Region
    End Class

End Namespace
