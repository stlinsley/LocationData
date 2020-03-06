using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocationData.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CombinedLocationData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Alpha2Code = table.Column<string>(nullable: true),
                    Alpha3Code = table.Column<string>(nullable: true),
                    Population = table.Column<long>(nullable: false),
                    Latitude = table.Column<int>(nullable: false),
                    Longitude = table.Column<int>(nullable: false),
                    Weather = table.Column<string>(nullable: true),
                    TouristRating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombinedLocationData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true),
                    CombinedLocationDataId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currency_CombinedLocationData_CombinedLocationDataId",
                        column: x => x.CombinedLocationDataId,
                        principalTable: "CombinedLocationData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CombinedLocationDataId",
                table: "Currency",
                column: "CombinedLocationDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "CombinedLocationData");
        }
    }
}
