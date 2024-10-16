using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddDominio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Espacios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminEspacioId = table.Column<int>(type: "int", nullable: false),
                    NombreEspacio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Espacios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Espacios_Usuarios_AdminEspacioId",
                        column: x => x.AdminEspacioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EspacioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Espacios_EspacioId",
                        column: x => x.EspacioId,
                        principalTable: "Espacios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EspacioId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoInicial = table.Column<double>(type: "float", nullable: true),
                    UltimosDigitos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BancoEmisor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditoDisponible = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuentas_Espacios_EspacioId",
                        column: x => x.EspacioId,
                        principalTable: "Espacios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EspacioUsuario",
                columns: table => new
                {
                    MiembrosEspacioId = table.Column<int>(type: "int", nullable: false),
                    espaciosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspacioUsuario", x => new { x.MiembrosEspacioId, x.espaciosId });
                    table.ForeignKey(
                        name: "FK_EspacioUsuario_Espacios_espaciosId",
                        column: x => x.espaciosId,
                        principalTable: "Espacios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EspacioUsuario_Usuarios_MiembrosEspacioId",
                        column: x => x.MiembrosEspacioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Objetivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoMaximo = table.Column<double>(type: "float", nullable: false),
                    GastoActual = table.Column<double>(type: "float", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URLHabilitada = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: true),
                    EspacioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objetivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objetivos_Espacios_EspacioId",
                        column: x => x.EspacioId,
                        principalTable: "Espacios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Objetivos_Usuarios_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TiposDeCambio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cotizacion = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EspacioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDeCambio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposDeCambio_Espacios_EspacioId",
                        column: x => x.EspacioId,
                        principalTable: "Espacios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    TipoTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EspacioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transacciones_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transacciones_Espacios_EspacioId",
                        column: x => x.EspacioId,
                        principalTable: "Espacios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EspacioId",
                table: "Categorias",
                column: "EspacioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_EspacioId",
                table: "Cuentas",
                column: "EspacioId");

            migrationBuilder.CreateIndex(
                name: "IX_Espacios_AdminEspacioId",
                table: "Espacios",
                column: "AdminEspacioId");

            migrationBuilder.CreateIndex(
                name: "IX_EspacioUsuario_espaciosId",
                table: "EspacioUsuario",
                column: "espaciosId");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_EspacioId",
                table: "Objetivos",
                column: "EspacioId");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_UsuarioCreadorId",
                table: "Objetivos",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposDeCambio_EspacioId",
                table: "TiposDeCambio",
                column: "EspacioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_CategoriaId",
                table: "Transacciones",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_CuentaId",
                table: "Transacciones",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_EspacioId",
                table: "Transacciones",
                column: "EspacioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EspacioUsuario");

            migrationBuilder.DropTable(
                name: "Objetivos");

            migrationBuilder.DropTable(
                name: "TiposDeCambio");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "Espacios");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
