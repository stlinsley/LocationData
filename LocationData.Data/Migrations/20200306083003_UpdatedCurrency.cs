using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocationData.Data.Migrations
{
    public partial class UpdatedCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currency_CombinedLocationData_CombinedLocationDataId",
                table: "Currency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_Currency_CombinedLocationDataId",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CombinedLocationDataId",
                table: "Currency");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Currency",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "CombinedLocationData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CombinedLocationData_CurrencyId",
                table: "CombinedLocationData",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CombinedLocationData_Currency_CurrencyId",
                table: "CombinedLocationData",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CombinedLocationData_Currency_CurrencyId",
                table: "CombinedLocationData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_CombinedLocationData_CurrencyId",
                table: "CombinedLocationData");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "CombinedLocationData");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Currency",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "CombinedLocationDataId",
                table: "Currency",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CombinedLocationDataId",
                table: "Currency",
                column: "CombinedLocationDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_CombinedLocationData_CombinedLocationDataId",
                table: "Currency",
                column: "CombinedLocationDataId",
                principalTable: "CombinedLocationData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
