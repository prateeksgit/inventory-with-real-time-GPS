using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class reCleanDeviceAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.CreateTable(
                name: "arkbo_deviceassignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<int>(type: "integer", nullable: false),
                    AssignedById = table.Column<int>(type: "integer", nullable: false),
                    AssignedToId = table.Column<int>(type: "integer", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arkbo_deviceassignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_arkbo_deviceassignments_arkbo_devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "arkbo_devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_arkbo_deviceassignments_arkbo_users_AssignedById",
                        column: x => x.AssignedById,
                        principalTable: "arkbo_users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_arkbo_deviceassignments_arkbo_users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "arkbo_users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_deviceassignments_AssignedById",
                table: "arkbo_deviceassignments",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_deviceassignments_AssignedToId",
                table: "arkbo_deviceassignments",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_deviceassignments_DeviceId",
                table: "arkbo_deviceassignments",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById",
                principalTable: "arkbo_users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.DropTable(
                name: "arkbo_deviceassignments");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById",
                principalTable: "arkbo_users",
                principalColumn: "UserId");
        }
    }
}
