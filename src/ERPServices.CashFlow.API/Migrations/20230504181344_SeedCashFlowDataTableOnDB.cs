using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPServices.CashFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCashFlowDataTableOnDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "cash_flow",
                columns: new[] { "id", "date_inc", "description", "type", "value" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lancamento Recebimento cliente 3", "D", 100m },
                    { 2L, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lancamento Recebimento cliente 2", "D", 100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "cash_flow",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "cash_flow",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
