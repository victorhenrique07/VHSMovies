using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class atualizando_tabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "People");

            migrationBuilder.CreateTable(
                name: "PersonRoleMapping",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRoleMapping", x => new { x.PersonId, x.Role });
                    table.ForeignKey(
                        name: "FK_PersonRoleMapping_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonRoleMapping");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
