using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabalho_Lanches.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedidos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consumiveis",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoriaID = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    composicao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    valor = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumiveis", x => x.id);
                    table.ForeignKey(
                        name: "FK_consumiveis_categorias_categoriaID",
                        column: x => x.categoriaID,
                        principalTable: "categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "itensPedidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pedID = table.Column<int>(type: "int", nullable: false),
                    consumivelID = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    virouPedido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itensPedidos", x => x.id);
                    table.ForeignKey(
                        name: "FK_itensPedidos_consumiveis_consumivelID",
                        column: x => x.consumivelID,
                        principalTable: "consumiveis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_consumiveis_categoriaID",
                table: "consumiveis",
                column: "categoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_itensPedidos_consumivelID",
                table: "itensPedidos",
                column: "consumivelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itensPedidos");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "consumiveis");

            migrationBuilder.DropTable(
                name: "categorias");
        }
    }
}
