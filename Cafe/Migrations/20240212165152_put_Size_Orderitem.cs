using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Migrations
{
    public partial class put_Size_Orderitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "OrderItems",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
