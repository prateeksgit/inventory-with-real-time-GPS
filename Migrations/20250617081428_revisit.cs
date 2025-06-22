using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class revisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.AlterColumn<int>(
                name: "createdById",
                table: "arkbo_users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById",
                principalTable: "arkbo_users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.AlterColumn<int>(
                name: "createdById",
                table: "arkbo_users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById",
                principalTable: "arkbo_users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
