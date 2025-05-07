using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VHSMovies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingrelevancefieldtomap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Relevance",
                table: "titles",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Relevance",
                table: "titles",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)");
        }
    }
}
