using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class titles_genres_foreign_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId1",
                table: "TitlesGenres",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TitlesGenres_GenreId1",
                table: "TitlesGenres",
                column: "GenreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TitlesGenres_Genres_GenreId1",
                table: "TitlesGenres",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TitlesGenres_Genres_GenreId1",
                table: "TitlesGenres");

            migrationBuilder.DropIndex(
                name: "IX_TitlesGenres_GenreId1",
                table: "TitlesGenres");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "TitlesGenres");
        }
    }
}
