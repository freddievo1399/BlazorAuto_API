using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuto_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20250810013902 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarouselImagesJson",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarouselImagesJson",
                table: "Product");
        }
    }
}
