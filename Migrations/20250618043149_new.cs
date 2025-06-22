using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_userdevices_arkbo_devices_DeviceId1",
                table: "arkbo_userdevices");

            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_userdevices_arkbo_users_UserId1",
                table: "arkbo_userdevices");

            migrationBuilder.DropIndex(
                name: "IX_arkbo_userdevices_DeviceId1",
                table: "arkbo_userdevices");

            migrationBuilder.DropIndex(
                name: "IX_arkbo_userdevices_UserId1",
                table: "arkbo_userdevices");

            migrationBuilder.DropColumn(
                name: "DeviceId1",
                table: "arkbo_userdevices");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "arkbo_userdevices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId1",
                table: "arkbo_userdevices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "arkbo_userdevices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_userdevices_DeviceId1",
                table: "arkbo_userdevices",
                column: "DeviceId1");

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_userdevices_UserId1",
                table: "arkbo_userdevices",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_userdevices_arkbo_devices_DeviceId1",
                table: "arkbo_userdevices",
                column: "DeviceId1",
                principalTable: "arkbo_devices",
                principalColumn: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_userdevices_arkbo_users_UserId1",
                table: "arkbo_userdevices",
                column: "UserId1",
                principalTable: "arkbo_users",
                principalColumn: "UserId");
        }
    }
}
