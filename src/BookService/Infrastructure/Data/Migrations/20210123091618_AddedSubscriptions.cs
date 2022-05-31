using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class AddedSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReceiptData = table.Column<string>(type: "text", nullable: true),
                    UserAccountUserName = table.Column<string>(type: "text", nullable: false),
                    OriginalPurchaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CancellationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_UserAccounts_UserAccountUserName",
                        column: x => x.UserAccountUserName,
                        principalTable: "UserAccounts",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_UserAccountUserName",
                table: "Subscription",
                column: "UserAccountUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
