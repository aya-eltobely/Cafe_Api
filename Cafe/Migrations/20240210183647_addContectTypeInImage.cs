using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Migrations
{
    public partial class addContectTypeInImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Images");
        }
    }
}
