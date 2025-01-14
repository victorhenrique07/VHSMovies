using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Titles");

            migrationBuilder.AddColumn<string>(
                name: "TitleExternalId",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleExternalId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Titles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Titles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
