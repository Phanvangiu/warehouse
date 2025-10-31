using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace warehouse.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistories_Users_EmployeeId",
                table: "PositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_ManagerId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_ManagerId1",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreUsers_Stores_StoreId",
                table: "StoreUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreUsers_Stores_StoreId1",
                table: "StoreUsers");

            migrationBuilder.DropIndex(
                name: "IX_StoreUsers_StoreId1",
                table: "StoreUsers");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ManagerId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ManagerId1",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreId1",
                table: "StoreUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ManagerId1",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "Tilte",
                table: "Positions",
                newName: "Title");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "StoreUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId1",
                table: "PositionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId1",
                table: "Products",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistories_PositionId1",
                table: "PositionHistories",
                column: "PositionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistories_Positions_PositionId1",
                table: "PositionHistories",
                column: "PositionId1",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistories_Users_EmployeeId",
                table: "PositionHistories",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId1",
                table: "Products",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreUsers_Stores_StoreId",
                table: "StoreUsers",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistories_Positions_PositionId1",
                table: "PositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistories_Users_EmployeeId",
                table: "PositionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId1",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreUsers_Stores_StoreId",
                table: "StoreUsers");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_PositionHistories_PositionId1",
                table: "PositionHistories");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PositionId1",
                table: "PositionHistories");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Positions",
                newName: "Tilte");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "StoreUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId1",
                table: "StoreUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId1",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreUsers_StoreId1",
                table: "StoreUsers",
                column: "StoreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ManagerId",
                table: "Stores",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ManagerId1",
                table: "Stores",
                column: "ManagerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistories_Users_EmployeeId",
                table: "PositionHistories",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_ManagerId",
                table: "Stores",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_ManagerId1",
                table: "Stores",
                column: "ManagerId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreUsers_Stores_StoreId",
                table: "StoreUsers",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreUsers_Stores_StoreId1",
                table: "StoreUsers",
                column: "StoreId1",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
