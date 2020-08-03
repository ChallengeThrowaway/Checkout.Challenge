using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    CardDetailsId = table.Column<Guid>(nullable: false),
                    CardNumber = table.Column<string>(nullable: true),
                    Ccv = table.Column<string>(nullable: true),
                    CardholderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.CardDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CurrencyIsoAlpha3 = table.Column<string>(nullable: true),
                    CardDetailsId = table.Column<Guid>(nullable: true),
                    BankId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_CardDetails_CardDetailsId",
                        column: x => x.CardDetailsId,
                        principalTable: "CardDetails",
                        principalColumn: "CardDetailsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    PaymentStatusId = table.Column<Guid>(nullable: false),
                    StatusKey = table.Column<int>(nullable: false),
                    StatusDateTime = table.Column<DateTimeOffset>(nullable: false),
                    PaymentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.PaymentStatusId);
                    table.ForeignKey(
                        name: "FK_PaymentStatuses_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardDetailsId",
                table: "Payments",
                column: "CardDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_PaymentId",
                table: "PaymentStatuses",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "CardDetails");
        }
    }
}
