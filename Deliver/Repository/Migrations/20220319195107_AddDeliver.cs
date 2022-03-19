using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class AddDeliver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Delivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliverStatus = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<long>(type: "bigint", nullable: false),
                    FromId = table.Column<long>(type: "bigint", nullable: false),
                    ToId = table.Column<long>(type: "bigint", nullable: false),
                    ToId1 = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivers_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Delivers_Locations_ToId",
                        column: x => x.ToId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
                    

            migrationBuilder.CreateIndex(
                name: "IX_Delivers_CarId",
                table: "Delivers",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivers_ToId",
                table: "Delivers",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivers_ToId1",
                table: "Delivers",
                column: "ToId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delivers");
        }
    }
}
