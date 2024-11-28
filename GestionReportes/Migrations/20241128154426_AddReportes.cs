using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionReportes.Migrations
{
    /// <inheritdoc />
    public partial class AddReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombreArchivo = table.Column<string>(type: "text", nullable: false),
                    ruta = table.Column<string>(type: "text", nullable: false),
                    fechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosReporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosReporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposReporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposReporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rut = table.Column<int>(type: "integer", nullable: false),
                    dv = table.Column<char>(type: "character(1)", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<int>(type: "integer", nullable: false),
                    contraseña = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FuncionariosMunicipal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    departamento = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionariosMunicipal", x => x.id);
                    table.ForeignKey(
                        name: "FK_FuncionariosMunicipal_Usuarios_id",
                        column: x => x.id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vecinos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vecinos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vecinos_Usuarios_id",
                        column: x => x.id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Imagen = table.Column<string>(type: "text", nullable: false),
                    idVecino = table.Column<int>(type: "integer", nullable: false),
                    Vecinoid = table.Column<int>(type: "integer", nullable: false),
                    idEstado = table.Column<int>(type: "integer", nullable: false),
                    EstadoId = table.Column<int>(type: "integer", nullable: false),
                    idTipo = table.Column<int>(type: "integer", nullable: false),
                    TipoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reportes_EstadosReporte_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadosReporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reportes_TiposReporte_TipoId",
                        column: x => x.TipoId,
                        principalTable: "TiposReporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reportes_Vecinos_Vecinoid",
                        column: x => x.Vecinoid,
                        principalTable: "Vecinos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialReportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idReporte = table.Column<int>(type: "integer", nullable: false),
                    idFuncionario = table.Column<int>(type: "integer", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observacion = table.Column<string>(type: "text", nullable: false),
                    ReporteId = table.Column<int>(type: "integer", nullable: false),
                    Funcionarioid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialReportes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialReportes_FuncionariosMunicipal_Funcionarioid",
                        column: x => x.Funcionarioid,
                        principalTable: "FuncionariosMunicipal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialReportes_Reportes_ReporteId",
                        column: x => x.ReporteId,
                        principalTable: "Reportes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReportes_Funcionarioid",
                table: "HistorialReportes",
                column: "Funcionarioid");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialReportes_ReporteId",
                table: "HistorialReportes",
                column: "ReporteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_EstadoId",
                table: "Reportes",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_TipoId",
                table: "Reportes",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_Vecinoid",
                table: "Reportes",
                column: "Vecinoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "HistorialReportes");

            migrationBuilder.DropTable(
                name: "FuncionariosMunicipal");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "EstadosReporte");

            migrationBuilder.DropTable(
                name: "TiposReporte");

            migrationBuilder.DropTable(
                name: "Vecinos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
