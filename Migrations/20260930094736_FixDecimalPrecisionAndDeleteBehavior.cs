using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanlyPhongtapGymFitnessClub.Migrations
{
    /// <inheritdoc />
    public partial class FixDecimalPrecisionAndDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users",
                column: "AssignedTrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users",
                column: "AssignedTrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }
    }
}
