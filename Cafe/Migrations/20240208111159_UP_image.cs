using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Migrations
{
    public partial class UP_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_SubCategories_SubCategoryId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_SubCategoryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_ImageId",
                table: "SubCategories",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageId",
                table: "Products",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Images_ImageId",
                table: "Products",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Images_ImageId",
                table: "SubCategories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Images_ImageId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Images_ImageId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_ImageId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_SubCategoryId",
                table: "Images",
                column: "SubCategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                table: "Images",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_SubCategories_SubCategoryId",
                table: "Images",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }
    }
}
