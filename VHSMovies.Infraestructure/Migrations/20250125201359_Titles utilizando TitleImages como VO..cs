using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class TitlesutilizandoTitleImagescomoVO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TitleImages");

            migrationBuilder.AddColumn<string>(
                name: "PosterImageUrl",
                table: "Titles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrincipalImageUrl",
                table: "Titles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterImageUrl",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "PrincipalImageUrl",
                table: "Titles");

            migrationBuilder.CreateTable(
                name: "TitleImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    PosterImageUrl = table.Column<string>(type: "text", nullable: false),
                    PrincipalImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TitleImages_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TitleImages_TitleId",
                table: "TitleImages",
                column: "TitleId");
        }
    }
}
