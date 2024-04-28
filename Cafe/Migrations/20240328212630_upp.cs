using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Migrations
{
    public partial class upp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "OrderItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "OrderItems",
                type: "int",
                nullable: true);
        }
    }
}
