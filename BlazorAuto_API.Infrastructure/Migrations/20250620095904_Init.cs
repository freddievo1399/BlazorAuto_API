using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuto_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", unicode: false, maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", unicode: false, maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", unicode: false, maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Category_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Specification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", unicode: false, maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", unicode: false, maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Specification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Specification_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Guid",
                table: "Category",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Guid",
                table: "Product",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_CategoryId",
                table: "Product_Category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Guid",
                table: "Product_Category",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_ProductId_CategoryId",
                table: "Product_Category",
                columns: new[] { "ProductId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Specification_Guid",
                table: "Product_Specification",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Specification_ProductId",
                table: "Product_Specification",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product_Category");

            migrationBuilder.DropTable(
                name: "Product_Specification");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
