using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class GameRefactor : Migration
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

            migrationBuilder.DropColumn(
                name: "MinimumId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RecommendedId",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "GameGalleries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games",
                column: "MinimumRequirementId",
                principalTable: "MinimumRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games",
                column: "RecommendedRequirementId",
                principalTable: "RecommendedRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.AddColumn<Guid>(
                name: "MinimumId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecommendedId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "GameGalleries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
