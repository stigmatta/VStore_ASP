using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class NullReqAllowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MinimumRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MinimumId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games",
                column: "MinimumRequirementId",
                principalTable: "MinimumRequirements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games",
                column: "RecommendedRequirementId",
                principalTable: "RecommendedRequirements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RecommendedId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MinimumRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MinimumId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games",
                column: "MinimumRequirementId",
                principalTable: "MinimumRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games",
                column: "RecommendedRequirementId",
                principalTable: "RecommendedRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
