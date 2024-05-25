using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreacionCervezas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cervezas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alcohol = table.Column<double>(type: "float", nullable: false),
                    idEstilo = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cervezas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cervezas_Estilos_idEstilo",
                        column: x => x.idEstilo,
                        principalTable: "Estilos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cervezas_idEstilo",
                table: "Cervezas",
                column: "idEstilo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cervezas");
        }
    }
}
