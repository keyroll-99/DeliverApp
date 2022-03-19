using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddDeliver2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Users_DriverId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivers_Locations_ToId",
                table: "Delivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Company_CompanyId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Company_CompanyId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Users_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivers_Locations_ToId",
                table: "Delivers",
                column: "ToId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Company_CompanyId",
                table: "Locations",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Company_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Users_DriverId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivers_Locations_ToId",
                table: "Delivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Company_CompanyId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Company_CompanyId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Users_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivers_Locations_ToId",
                table: "Delivers",
                column: "ToId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Company_CompanyId",
                table: "Locations",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Company_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
