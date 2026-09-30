using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanlyPhongtapGymFitnessClub.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedMemberIds",
                table: "Trainers");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AssignedTrainerId",
                table: "Users",
                column: "AssignedTrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users",
                column: "AssignedTrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Trainers_AssignedTrainerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AssignedTrainerId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AssignedMemberIds",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
