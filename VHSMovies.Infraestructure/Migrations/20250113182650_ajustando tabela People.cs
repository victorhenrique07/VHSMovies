using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ajustandotabelaPeople : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonRoleMapping");

            migrationBuilder.DropIndex(
                name: "IX_People_ExternalId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "People");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Casts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Casts");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "People",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_People_ExternalId",
                table: "People",
                column: "ExternalId",
                unique: true);
        }
    }
}
