using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SolveRelationIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Cards_CardId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardId1",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardId1",
                table: "Payments",
                column: "CardId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Cards_CardId1",
                table: "Payments",
                column: "CardId1",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
