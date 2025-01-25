using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class adicionandomapeamentoparaTitleImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Title_Images_Titles_TitleId",
                table: "Title_Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Title_Images",
                table: "Title_Images");

            migrationBuilder.RenameTable(
                name: "Title_Images",
                newName: "TitleImages");

            migrationBuilder.RenameIndex(
                name: "IX_Title_Images_TitleId",
                table: "TitleImages",
                newName: "IX_TitleImages_TitleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TitleImages",
                table: "TitleImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TitleImages_Titles_TitleId",
                table: "TitleImages",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TitleImages_Titles_TitleId",
                table: "TitleImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TitleImages",
                table: "TitleImages");

            migrationBuilder.RenameTable(
                name: "TitleImages",
                newName: "Title_Images");

            migrationBuilder.RenameIndex(
                name: "IX_TitleImages_TitleId",
                table: "Title_Images",
                newName: "IX_Title_Images_TitleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Title_Images",
                table: "Title_Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Title_Images_Titles_TitleId",
                table: "Title_Images",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
