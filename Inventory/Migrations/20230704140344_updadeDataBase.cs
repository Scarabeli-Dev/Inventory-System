using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    public partial class updadeDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_Item_ItemId",
                table: "InventoryMovement");

            migrationBuilder.DropTable(
                name: "ItemsStockTaking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryMovement",
                table: "InventoryMovement");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovement_ItemId",
                table: "InventoryMovement");

            migrationBuilder.UpdateData(
                table: "InventoryMovement",
                keyColumn: "ItemId",
                keyValue: null,
                column: "ItemId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "InventoryMovement",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "InventoryMovement",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "InventoryMovement",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ItemId1",
                table: "InventoryMovement",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryMovement",
                table: "InventoryMovement",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovement_ItemId1",
                table: "InventoryMovement",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_Item_ItemId1",
                table: "InventoryMovement",
                column: "ItemId1",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_Item_ItemId1",
                table: "InventoryMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryMovement",
                table: "InventoryMovement");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovement_ItemId1",
                table: "InventoryMovement");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "InventoryMovement");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "InventoryMovement",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "InventoryMovement",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "InventoryMovement",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryMovement",
                table: "InventoryMovement",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemsStockTaking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InventoryStartId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemCountEnded = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ItemCountRealized = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsStockTaking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsStockTaking_InventoryStart_InventoryStartId",
                        column: x => x.InventoryStartId,
                        principalTable: "InventoryStart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsStockTaking_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovement_ItemId",
                table: "InventoryMovement",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsStockTaking_InventoryStartId",
                table: "ItemsStockTaking",
                column: "InventoryStartId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsStockTaking_ItemId",
                table: "ItemsStockTaking",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_Item_ItemId",
                table: "InventoryMovement",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id");
        }
    }
}
