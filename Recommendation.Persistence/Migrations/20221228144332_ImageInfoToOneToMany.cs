using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImageInfoToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageInfo_ReviewId",
                table: "ImageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfo_ReviewId",
                table: "ImageInfo",
                column: "ReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImageInfo_ReviewId",
                table: "ImageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_ImageInfo_ReviewId",
                table: "ImageInfo",
                column: "ReviewId",
                unique: true);
        }
    }
}
