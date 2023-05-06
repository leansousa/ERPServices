using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPServices.Report.API.Migrations
{
    /// <inheritdoc />
    public partial class AddReportCashFlowDataTableOnDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cash_flow_daily_report",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    date_inc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    total_debit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    total_credit = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_flow_daily_report", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "cash_flow_daily_report",
                columns: new[] { "id", "date_inc", "total_credit", "total_debit" },
                values: new object[,]
                {
                    { 1L, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, 200m },
                    { 2L, new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 140m, 210m },
                    { 3L, new DateTime(2022, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000m, 2000m },
                    { 4L, new DateTime(2022, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 160m, 0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cash_flow_daily_report");
        }
    }
}
