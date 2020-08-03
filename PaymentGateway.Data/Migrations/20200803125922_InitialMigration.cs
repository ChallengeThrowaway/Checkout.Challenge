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
                    Cvv = table.Column<string>(nullable: true),
                    CardholderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.CardDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantId = table.Column<Guid>(nullable: false),
                    MerchantName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    ApiKeyId = table.Column<Guid>(nullable: false),
                    OwnerMerchantId = table.Column<Guid>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    ValidFrom = table.Column<DateTimeOffset>(nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.ApiKeyId);
                    table.ForeignKey(
                        name: "FK_ApiKeys_Merchants_OwnerMerchantId",
                        column: x => x.OwnerMerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CurrencyIsoAlpha3 = table.Column<string>(nullable: true),
                    CardDetailsId = table.Column<Guid>(nullable: true),
                    BankId = table.Column<Guid>(nullable: true),
                    MerchantId = table.Column<Guid>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Payments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
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
                name: "IX_ApiKeys_OwnerMerchantId",
                table: "ApiKeys",
                column: "OwnerMerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardDetailsId",
                table: "Payments",
                column: "CardDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantId",
                table: "Payments",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_PaymentId",
                table: "PaymentStatuses",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
