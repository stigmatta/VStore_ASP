using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class UniqueEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUsers_Users_BlockedUserId",
                table: "BlockedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUsers_Users_UserId",
                table: "BlockedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedUsers",
                table: "BlockedUsers");

            migrationBuilder.RenameTable(
                name: "BlockedUsers",
                newName: "BlockedUser");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedUsers_BlockedUserId",
                table: "BlockedUser",
                newName: "IX_BlockedUser_BlockedUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedUser",
                table: "BlockedUser",
                columns: new[] { "UserId", "BlockedUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUser_Users_BlockedUserId",
                table: "BlockedUser",
                column: "BlockedUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUser_Users_UserId",
                table: "BlockedUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUser_Users_BlockedUserId",
                table: "BlockedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUser_Users_UserId",
                table: "BlockedUser");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockedUser",
                table: "BlockedUser");

            migrationBuilder.RenameTable(
                name: "BlockedUser",
                newName: "BlockedUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedUser_BlockedUserId",
                table: "BlockedUsers",
                newName: "IX_BlockedUsers_BlockedUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockedUsers",
                table: "BlockedUsers",
                columns: new[] { "UserId", "BlockedUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUsers_Users_BlockedUserId",
                table: "BlockedUsers",
                column: "BlockedUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUsers_Users_UserId",
                table: "BlockedUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
