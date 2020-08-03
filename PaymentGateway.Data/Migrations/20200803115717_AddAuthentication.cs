using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Data.Migrations
{
    public partial class AddAuthentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ApiKeyId = table.Column<Guid>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_OwnerMerchantId",
                table: "ApiKeys",
                column: "OwnerMerchantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
