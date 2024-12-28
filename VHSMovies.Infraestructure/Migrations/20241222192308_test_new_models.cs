using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class test_new_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "reviewers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviewers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "person_role",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_role", x => new { x.PersonId, x.Role });
                    table.ForeignKey(
                        name: "FK_person_role_people_PersonId",
                        column: x => x.PersonId,
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "casts",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_casts", x => new { x.TitleId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_casts_people_PersonId",
                        column: x => x.PersonId,
                        principalTable: "people",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_casts_titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movies_titles_Id",
                        column: x => x.Id,
                        principalTable: "titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    ReviewerId = table.Column<int>(type: "integer", nullable: false),
                    TitleExternalId = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    TitleExternalUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Rating = table.Column<decimal>(type: "numeric(2,1)", precision: 2, scale: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reviews_reviewers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "reviewers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "titles_genres",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titles_genres", x => new { x.TitleId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_titles_genres_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_titles_genres_titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tvshows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tvshows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tvshows_titles_Id",
                        column: x => x.Id,
                        principalTable: "titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tvshow_seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EpisodesQuantity = table.Column<int>(type: "integer", nullable: false),
                    TVShowId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tvshow_seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tvshow_seasons_tvshows_TVShowId",
                        column: x => x.TVShowId,
                        principalTable: "tvshows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_casts_PersonId",
                table: "casts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_ReviewerId",
                table: "reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_TitleId",
                table: "reviews",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_titles_genres_GenreId",
                table: "titles_genres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_tvshow_seasons_TVShowId",
                table: "tvshow_seasons",
                column: "TVShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "casts");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "person_role");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "titles_genres");

            migrationBuilder.DropTable(
                name: "tvshow_seasons");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "reviewers");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "tvshows");

            migrationBuilder.DropTable(
                name: "titles");
        }
    }
}
