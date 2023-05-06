using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPServices.CashFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCashFlowDataTableOnDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cash_flow",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    type = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_inc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_flow", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "cash_flow",
                columns: new[] { "id", "date_inc", "description", "type", "value" },
                values: new object[,]
                {
                    { 3L, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lancamento Folha de Pagamento", "C", 10m },
                    { 4L, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lancamento Recebimento cliente", "D", 100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cash_flow");
        }
    }
}
