using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullableOnDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "arkbo_devices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices",
                column: "UserId",
                principalTable: "arkbo_users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "arkbo_devices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices",
                column: "UserId",
                principalTable: "arkbo_users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
