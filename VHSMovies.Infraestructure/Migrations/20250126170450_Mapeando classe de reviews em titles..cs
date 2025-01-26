using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Mapeandoclassedereviewsemtitles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Titles_Id",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Reviews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TitleId",
                table: "Reviews",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Titles_TitleId",
                table: "Reviews",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Titles_TitleId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_TitleId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Reviews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Titles_Id",
                table: "Reviews",
                column: "Id",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
