using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCompositionToCompositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Composition_Reviews_ReviewId",
                table: "Composition");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Composition_CompositionId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Composition",
                table: "Composition");

            migrationBuilder.RenameTable(
                name: "Composition",
                newName: "Compositions");

            migrationBuilder.RenameIndex(
                name: "IX_Composition_ReviewId",
                table: "Compositions",
                newName: "IX_Compositions_ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Reviews_ReviewId",
                table: "Compositions",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Compositions_CompositionId",
                table: "Ratings",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Reviews_ReviewId",
                table: "Compositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Compositions_CompositionId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions");

            migrationBuilder.RenameTable(
                name: "Compositions",
                newName: "Composition");

            migrationBuilder.RenameIndex(
                name: "IX_Compositions_ReviewId",
                table: "Composition",
                newName: "IX_Composition_ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Composition",
                table: "Composition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Composition_Reviews_ReviewId",
                table: "Composition",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Composition_CompositionId",
                table: "Ratings",
                column: "CompositionId",
                principalTable: "Composition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
