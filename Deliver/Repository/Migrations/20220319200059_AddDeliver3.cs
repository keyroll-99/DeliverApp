using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddDeliver3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_Delivers_FromId",
                table: "Delivers",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivers_Locations_FromId",
                table: "Delivers",
                column: "FromId",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivers_Locations_FromId",
                table: "Delivers");

            migrationBuilder.DropIndex(
                name: "IX_Delivers_FromId",
                table: "Delivers");

            migrationBuilder.AddColumn<long>(
                name: "ToId1",
                table: "Delivers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Delivers_ToId1",
                table: "Delivers",
                column: "ToId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivers_Locations_ToId1",
                table: "Delivers",
                column: "ToId1",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
