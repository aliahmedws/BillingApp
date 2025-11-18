using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billing.Migrations
{
    /// <inheritdoc />
    public partial class TarrifSlabTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTarrifSlabs_CreationTime",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "RateRangeEight",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "RateRangeFive",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "RateRangeFour",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "RateRangeOne",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "RateRangeSeven",
                table: "AppTarrifSlabs");

            migrationBuilder.RenameColumn(
                name: "RateRangeTwo",
                table: "AppTarrifSlabs",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "RateRangeThree",
                table: "AppTarrifSlabs",
                newName: "LowerSlab");

            migrationBuilder.RenameColumn(
                name: "RateRangeSix",
                table: "AppTarrifSlabs",
                newName: "UpperSlab");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppTarrifSlabs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTarrifSlabs_TenantId",
                table: "AppTarrifSlabs",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTarrifSlabs_TenantId",
                table: "AppTarrifSlabs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppTarrifSlabs");

            migrationBuilder.RenameColumn(
                name: "UpperSlab",
                table: "AppTarrifSlabs",
                newName: "RateRangeSix");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "AppTarrifSlabs",
                newName: "RateRangeTwo");

            migrationBuilder.RenameColumn(
                name: "LowerSlab",
                table: "AppTarrifSlabs",
                newName: "RateRangeThree");

            migrationBuilder.AddColumn<decimal>(
                name: "RateRangeEight",
                table: "AppTarrifSlabs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateRangeFive",
                table: "AppTarrifSlabs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateRangeFour",
                table: "AppTarrifSlabs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RateRangeOne",
                table: "AppTarrifSlabs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RateRangeSeven",
                table: "AppTarrifSlabs",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTarrifSlabs_CreationTime",
                table: "AppTarrifSlabs",
                column: "CreationTime");
        }
    }
}
