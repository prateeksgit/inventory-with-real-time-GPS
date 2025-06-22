using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace arkbo_inventory.Migrations
{
    /// <inheritdoc />
    public partial class NewMigratiobn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "createdById",
                table: "arkbo_users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById");

            migrationBuilder.AddForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users",
                column: "createdById",
                principalTable: "arkbo_users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_arkbo_users_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.DropIndex(
                name: "IX_arkbo_users_createdById",
                table: "arkbo_users");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "arkbo_users");
        }
    }
}
