using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class DeletedGameIdInReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_MinimumId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_RecommendedId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "RecommendedRequirements");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "MinimumRequirements");

            migrationBuilder.AddColumn<Guid>(
                name: "MinimumRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RecommendedRequirementId",
                table: "Games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Games_MinimumRequirementId",
                table: "Games",
                column: "MinimumRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RecommendedRequirementId",
                table: "Games",
                column: "RecommendedRequirementId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumRequirementId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedRequirementId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_MinimumRequirementId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_RecommendedRequirementId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MinimumRequirementId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RecommendedRequirementId",
                table: "Games");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "RecommendedRequirements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "MinimumRequirements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Games_MinimumId",
                table: "Games",
                column: "MinimumId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_RecommendedId",
                table: "Games",
                column: "RecommendedId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MinimumRequirements_MinimumId",
                table: "Games",
                column: "MinimumId",
                principalTable: "MinimumRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_RecommendedRequirements_RecommendedId",
                table: "Games",
                column: "RecommendedId",
                principalTable: "RecommendedRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
