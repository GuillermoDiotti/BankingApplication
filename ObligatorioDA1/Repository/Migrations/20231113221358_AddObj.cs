using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddObj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaObjetivosGastos",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ObjetivosGastosListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaObjetivosGastos", x => new { x.CategoriasId, x.ObjetivosGastosListId });
                    table.ForeignKey(
                        name: "FK_CategoriaObjetivosGastos_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaObjetivosGastos_Objetivos_ObjetivosGastosListId",
                        column: x => x.ObjetivosGastosListId,
                        principalTable: "Objetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaObjetivosGastos_ObjetivosGastosListId",
                table: "CategoriaObjetivosGastos",
                column: "ObjetivosGastosListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaObjetivosGastos");
        }
    }
}
