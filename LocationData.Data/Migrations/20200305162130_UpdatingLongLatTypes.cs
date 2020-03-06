using Microsoft.EntityFrameworkCore.Migrations;

namespace LocationData.Data.Migrations
{
    public partial class UpdatingLongLatTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "CombinedLocationData",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "CombinedLocationData",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Longitude",
                table: "CombinedLocationData",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<int>(
                name: "Latitude",
                table: "CombinedLocationData",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");
        }
    }
}
