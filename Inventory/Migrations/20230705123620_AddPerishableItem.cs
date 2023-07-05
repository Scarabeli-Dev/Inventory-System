using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    public partial class AddPerishableItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ExpirationDate",
                table: "StockTaking");

            migrationBuilder.DropColumn(
                name: "FabricationDate",
                table: "StockTaking");

            migrationBuilder.DropColumn(
                name: "ItemBatch",
                table: "StockTaking");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "InventoryMovement");

            migrationBuilder.AddColumn<bool>(
                name: "IsPerishableItem",
                table: "StockTaking",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "InventoryMovement",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "InventoryMovement",
                type: "varchar(150)",
                maxLength: 150,
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
                name: "PerishableItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FabricationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ItemBatch = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockTakingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerishableItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerishableItem_StockTaking_StockTakingId",
                        column: x => x.StockTakingId,
                        principalTable: "StockTaking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovement_ItemId",
                table: "InventoryMovement",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PerishableItem_StockTakingId",
                table: "PerishableItem",
                column: "StockTakingId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovement_Item_ItemId",
                table: "InventoryMovement",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovement_Item_ItemId",
                table: "InventoryMovement");

            migrationBuilder.DropTable(
                name: "PerishableItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryMovement",
                table: "InventoryMovement");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovement_ItemId",
                table: "InventoryMovement");

            migrationBuilder.DropColumn(
                name: "IsPerishableItem",
                table: "StockTaking");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "StockTaking",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FabricationDate",
                table: "StockTaking",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemBatch",
                table: "StockTaking",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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
                oldMaxLength: 150,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
