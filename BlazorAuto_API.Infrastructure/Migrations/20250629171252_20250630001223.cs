using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuto_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20250630001223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RichDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RichDescription",
                table: "Product");
        }
    }
}
