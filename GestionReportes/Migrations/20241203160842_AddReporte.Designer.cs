﻿// <auto-generated />
using System;
using GestionReportes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionReportes.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241203160842_AddReporte")]
    partial class AddReporte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GestionReportes.Models.Documento", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("fechaSubida")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("nombreArchivo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ruta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Documentos");
                });

            modelBuilder.Entity("GestionReportes.Models.EstadoReporte", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("EstadosReporte");
                });

            modelBuilder.Entity("GestionReportes.Models.FuncionarioMunicipal", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("integer");

                    b.Property<string>("departamento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("FuncionariosMunicipal");
                });

            modelBuilder.Entity("GestionReportes.Models.HistorialReporte", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateTime>("fecha")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("idFuncionario")
                        .HasColumnType("integer");

                    b.Property<int>("idReporte")
                        .HasColumnType("integer");

                    b.Property<string>("observacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("idFuncionario");

                    b.HasIndex("idReporte");

                    b.ToTable("HistorialReportes");
                });

            modelBuilder.Entity("GestionReportes.Models.Reporte", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("fechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("idEstado")
                        .HasColumnType("integer");

                    b.Property<int>("idTipo")
                        .HasColumnType("integer");

                    b.Property<int>("idVecino")
                        .HasColumnType("integer");

                    b.Property<int>("imagen")
                        .HasColumnType("integer");

                    b.Property<string>("ubicacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("idEstado");

                    b.HasIndex("idTipo");

                    b.HasIndex("idVecino");

                    b.ToTable("Reportes");
                });

            modelBuilder.Entity("GestionReportes.Models.TipoReporte", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("TiposReporte");
                });

            modelBuilder.Entity("GestionReportes.Models.Usuario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("contraseña")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<char>("dv")
                        .HasColumnType("character(1)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("rut")
                        .HasColumnType("integer");

                    b.Property<int>("telefono")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("GestionReportes.Models.Vecino", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("integer");

                    b.Property<string>("direccion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Vecinos");
                });

            modelBuilder.Entity("GestionReportes.Models.FuncionarioMunicipal", b =>
                {
                    b.HasOne("GestionReportes.Models.Usuario", "Usuario")
                        .WithOne()
                        .HasForeignKey("GestionReportes.Models.FuncionarioMunicipal", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionReportes.Models.HistorialReporte", b =>
                {
                    b.HasOne("GestionReportes.Models.FuncionarioMunicipal", "FuncionarioMunicipal")
                        .WithMany()
                        .HasForeignKey("idFuncionario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionReportes.Models.Reporte", "Reporte")
                        .WithMany("HistorialReportes")
                        .HasForeignKey("idReporte")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FuncionarioMunicipal");

                    b.Navigation("Reporte");
                });

            modelBuilder.Entity("GestionReportes.Models.Reporte", b =>
                {
                    b.HasOne("GestionReportes.Models.EstadoReporte", "Estado")
                        .WithMany()
                        .HasForeignKey("idEstado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionReportes.Models.TipoReporte", "Tipo")
                        .WithMany()
                        .HasForeignKey("idTipo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionReportes.Models.Vecino", "Vecino")
                        .WithMany("Reportes")
                        .HasForeignKey("idVecino")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");

                    b.Navigation("Tipo");

                    b.Navigation("Vecino");
                });

            modelBuilder.Entity("GestionReportes.Models.Vecino", b =>
                {
                    b.HasOne("GestionReportes.Models.Usuario", "Usuario")
                        .WithOne()
                        .HasForeignKey("GestionReportes.Models.Vecino", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionReportes.Models.Reporte", b =>
                {
                    b.Navigation("HistorialReportes");
                });

            modelBuilder.Entity("GestionReportes.Models.Vecino", b =>
                {
                    b.Navigation("Reportes");
                });
#pragma warning restore 612, 618
        }
    }
}