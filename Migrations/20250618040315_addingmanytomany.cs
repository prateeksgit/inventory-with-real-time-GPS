using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class addingmanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices");

            migrationBuilder.DropIndex(
                name: "IX_arkbo_devices_UserId",
                table: "arkbo_devices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "arkbo_devices");

            migrationBuilder.CreateTable(
                name: "arkbo_userdevices",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    DeviceId1 = table.Column<int>(type: "integer", nullable: true),
                    UserId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arkbo_userdevices", x => new { x.UserId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_arkbo_userdevices_arkbo_devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "arkbo_devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_arkbo_userdevices_arkbo_devices_DeviceId1",
                        column: x => x.DeviceId1,
                        principalTable: "arkbo_devices",
                        principalColumn: "DeviceId");
                    table.ForeignKey(
                        name: "FK_arkbo_userdevices_arkbo_users_UserId",
                        column: x => x.UserId,
                        principalTable: "arkbo_users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_arkbo_userdevices_arkbo_users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "arkbo_users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_userdevices_DeviceId",
                table: "arkbo_userdevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_userdevices_DeviceId1",
                table: "arkbo_userdevices",
                column: "DeviceId1");

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_userdevices_UserId1",
                table: "arkbo_userdevices",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arkbo_userdevices");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "arkbo_devices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_devices_UserId",
                table: "arkbo_devices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_devices_arkbo_users_UserId",
                table: "arkbo_devices",
                column: "UserId",
                principalTable: "arkbo_users",
                principalColumn: "UserId");
        }
    }
}
