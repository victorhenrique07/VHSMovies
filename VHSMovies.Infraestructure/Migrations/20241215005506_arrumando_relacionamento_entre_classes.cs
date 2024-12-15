using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class arrumando_relacionamento_entre_classes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Casts_Persons_ActorId",
                table: "Casts");

            migrationBuilder.DropForeignKey(
                name: "FK_Casts_Titles_TitleId",
                table: "Casts");

            migrationBuilder.DropForeignKey(
                name: "FK_TVShowSeasons_Titles_TVShowId",
                table: "TVShowSeasons");

            migrationBuilder.DropTable(
                name: "TitleDirectors");

            migrationBuilder.DropTable(
                name: "TitleGenres");

            migrationBuilder.DropTable(
                name: "TitleWriters");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Casts",
                table: "Casts");

            migrationBuilder.DropIndex(
                name: "IX_Casts_TitleId",
                table: "Casts");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Casts");

            migrationBuilder.RenameColumn(
                name: "TitleId",
                table: "Casts",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "CastId",
                table: "Titles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int[]>(
                name: "Genres",
                table: "Titles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Titles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Casts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Casts",
                table: "Casts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CastId = table.Column<int>(type: "integer", nullable: true),
                    CastId1 = table.Column<int>(type: "integer", nullable: true),
                    CastId2 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Casts_CastId",
                        column: x => x.CastId,
                        principalTable: "Casts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_People_Casts_CastId1",
                        column: x => x.CastId1,
                        principalTable: "Casts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_People_Casts_CastId2",
                        column: x => x.CastId2,
                        principalTable: "Casts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Titles_CastId",
                table: "Titles",
                column: "CastId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CastId",
                table: "People",
                column: "CastId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CastId1",
                table: "People",
                column: "CastId1");

            migrationBuilder.CreateIndex(
                name: "IX_People_CastId2",
                table: "People",
                column: "CastId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Casts_CastId",
                table: "Titles",
                column: "CastId",
                principalTable: "Casts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TVShowSeasons_Titles_TVShowId",
                table: "TVShowSeasons",
                column: "TVShowId",
                principalTable: "Titles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Casts_CastId",
                table: "Titles");

            migrationBuilder.DropForeignKey(
                name: "FK_TVShowSeasons_Titles_TVShowId",
                table: "TVShowSeasons");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropIndex(
                name: "IX_Titles_CastId",
                table: "Titles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Casts",
                table: "Casts");

            migrationBuilder.DropColumn(
                name: "CastId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Titles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Casts",
                newName: "TitleId");

            migrationBuilder.AlterColumn<int>(
                name: "TitleId",
                table: "Casts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ActorId",
                table: "Casts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Casts",
                table: "Casts",
                columns: new[] { "ActorId", "TitleId" });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleGenres",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleGenres", x => new { x.TitleId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_TitleGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleGenres_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TitleDirectors",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    DirectorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleDirectors", x => new { x.TitleId, x.DirectorId });
                    table.ForeignKey(
                        name: "FK_TitleDirectors_Persons_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleDirectors_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TitleWriters",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    WriterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleWriters", x => new { x.TitleId, x.WriterId });
                    table.ForeignKey(
                        name: "FK_TitleWriters_Persons_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleWriters_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Casts_TitleId",
                table: "Casts",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleDirectors_DirectorId",
                table: "TitleDirectors",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleGenres_GenreId",
                table: "TitleGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleWriters_WriterId",
                table: "TitleWriters",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Casts_Persons_ActorId",
                table: "Casts",
                column: "ActorId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Casts_Titles_TitleId",
                table: "Casts",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TVShowSeasons_Titles_TVShowId",
                table: "TVShowSeasons",
                column: "TVShowId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
