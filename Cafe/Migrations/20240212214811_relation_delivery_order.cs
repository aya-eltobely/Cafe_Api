using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Migrations
{
    public partial class relation_delivery_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_ApplicationUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_AppUserId",
                table: "Delivery",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Delivery_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "Delivery",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Delivery_DeliveryId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryId",
                table: "Orders");
        }
    }
}
