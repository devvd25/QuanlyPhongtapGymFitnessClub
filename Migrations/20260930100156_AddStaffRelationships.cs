using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanlyPhongtapGymFitnessClub.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredByStaffId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagedByStaffId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegisteredByStaffId",
                table: "Users",
                column: "RegisteredByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManagedByStaffId",
                table: "Products",
                column: "ManagedByStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Staffs_ManagedByStaffId",
                table: "Products",
                column: "ManagedByStaffId",
                principalTable: "Staffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Staffs_RegisteredByStaffId",
                table: "Users",
                column: "RegisteredByStaffId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Staffs_ManagedByStaffId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Staffs_RegisteredByStaffId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RegisteredByStaffId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManagedByStaffId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RegisteredByStaffId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ManagedByStaffId",
                table: "Products");
        }
    }
}
