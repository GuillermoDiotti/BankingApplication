﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(SqlContext))]
    [Migration("20231113220623_AddDominio")]
    partial class AddDominio
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dominio.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EspacioId")
                        .HasColumnType("int");

                    b.Property<string>("Estatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EspacioId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Dominio.Cuenta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EspacioId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Moneda")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EspacioId");

                    b.ToTable("Cuentas");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Cuenta");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Dominio.Espacio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdminEspacioId")
                        .HasColumnType("int");

                    b.Property<string>("NombreEspacio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminEspacioId");

                    b.ToTable("Espacios");
                });

            modelBuilder.Entity("Dominio.ObjetivosGastos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EspacioId")
                        .HasColumnType("int");

                    b.Property<double>("GastoActual")
                        .HasColumnType("float");

                    b.Property<double>("MontoMaximo")
                        .HasColumnType("float");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("URLHabilitada")
                        .HasColumnType("bit");

                    b.Property<int?>("UsuarioCreadorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EspacioId");

                    b.HasIndex("UsuarioCreadorId");

                    b.ToTable("Objetivos");
                });

            modelBuilder.Entity("Dominio.TipoDeCambio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Cotizacion")
                        .HasColumnType("float");

                    b.Property<int>("EspacioId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Moneda")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EspacioId");

                    b.ToTable("TiposDeCambio");
                });

            modelBuilder.Entity("Dominio.Transaccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("CuentaId")
                        .HasColumnType("int");

                    b.Property<int>("EspacioId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Moneda")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Monto")
                        .HasColumnType("float");

                    b.Property<string>("TipoTransaccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("CuentaId");

                    b.HasIndex("EspacioId");

                    b.ToTable("Transacciones");
                });

            modelBuilder.Entity("Dominio.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("EspacioUsuario", b =>
                {
                    b.Property<int>("MiembrosEspacioId")
                        .HasColumnType("int");

                    b.Property<int>("espaciosId")
                        .HasColumnType("int");

                    b.HasKey("MiembrosEspacioId", "espaciosId");

                    b.HasIndex("espaciosId");

                    b.ToTable("EspacioUsuario");
                });

            modelBuilder.Entity("Dominio.CuentaMonetaria", b =>
                {
                    b.HasBaseType("Dominio.Cuenta");

                    b.Property<double>("MontoInicial")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("CuentaMonetaria");
                });

            modelBuilder.Entity("Dominio.TarjetaCredito", b =>
                {
                    b.HasBaseType("Dominio.Cuenta");

                    b.Property<string>("BancoEmisor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CreditoDisponible")
                        .HasColumnType("float");

                    b.Property<DateTime>("FechaCierre")
                        .HasColumnType("datetime2");

                    b.Property<string>("UltimosDigitos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("TarjetaCredito");
                });

            modelBuilder.Entity("Dominio.Categoria", b =>
                {
                    b.HasOne("Dominio.Espacio", "Espacio")
                        .WithMany()
                        .HasForeignKey("EspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Espacio");
                });

            modelBuilder.Entity("Dominio.Cuenta", b =>
                {
                    b.HasOne("Dominio.Espacio", "Espacio")
                        .WithMany()
                        .HasForeignKey("EspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Espacio");
                });

            modelBuilder.Entity("Dominio.Espacio", b =>
                {
                    b.HasOne("Dominio.Usuario", "AdminEspacio")
                        .WithMany("espaciosAdministrados")
                        .HasForeignKey("AdminEspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminEspacio");
                });

            modelBuilder.Entity("Dominio.ObjetivosGastos", b =>
                {
                    b.HasOne("Dominio.Espacio", "Espacio")
                        .WithMany()
                        .HasForeignKey("EspacioId");

                    b.HasOne("Dominio.Usuario", "UsuarioCreador")
                        .WithMany()
                        .HasForeignKey("UsuarioCreadorId");

                    b.Navigation("Espacio");

                    b.Navigation("UsuarioCreador");
                });

            modelBuilder.Entity("Dominio.TipoDeCambio", b =>
                {
                    b.HasOne("Dominio.Espacio", "Espacio")
                        .WithMany()
                        .HasForeignKey("EspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Espacio");
                });

            modelBuilder.Entity("Dominio.Transaccion", b =>
                {
                    b.HasOne("Dominio.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Cuenta", "Cuenta")
                        .WithMany()
                        .HasForeignKey("CuentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Espacio", "Espacio")
                        .WithMany()
                        .HasForeignKey("EspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Cuenta");

                    b.Navigation("Espacio");
                });

            modelBuilder.Entity("EspacioUsuario", b =>
                {
                    b.HasOne("Dominio.Usuario", null)
                        .WithMany()
                        .HasForeignKey("MiembrosEspacioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Espacio", null)
                        .WithMany()
                        .HasForeignKey("espaciosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.Usuario", b =>
                {
                    b.Navigation("espaciosAdministrados");
                });
#pragma warning restore 612, 618
        }
    }
}
