using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class applying_unique_constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_People_ExternalId",
                table: "People",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_Id",
                table: "People",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_Id",
                table: "Casts",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_ExternalId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_Id",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Casts_Id",
                table: "Casts");
        }
    }
}
