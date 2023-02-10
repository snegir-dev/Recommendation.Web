using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class СhangedCompositionToOneToManyWithReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Reviews_ReviewId",
                table: "Compositions");

            migrationBuilder.DropIndex(
                name: "IX_Compositions_ReviewId",
                table: "Compositions");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Compositions");

            migrationBuilder.AddColumn<Guid>(
                name: "CompositionId",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CompositionId",
                table: "Reviews",
                column: "CompositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Compositions_CompositionId",
                table: "Reviews",
                column: "CompositionId",
                principalTable: "Compositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Compositions_CompositionId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CompositionId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CompositionId",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                table: "Compositions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Compositions_ReviewId",
                table: "Compositions",
                column: "ReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Reviews_ReviewId",
                table: "Compositions",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
