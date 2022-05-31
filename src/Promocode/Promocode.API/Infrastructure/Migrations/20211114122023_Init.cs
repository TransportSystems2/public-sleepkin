using Microsoft.EntityFrameworkCore.Migrations;

namespace PromoCode.API.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive", "und-u-ks-level2,und-u-ks-level2,icu,False");

            migrationBuilder.CreateTable(
                name: "PromoCode",
                columns: table => new
                {
                    PromoCode = table.Column<string>(type: "text", nullable: false, collation: "case_insensitive"),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Limit = table.Column<int>(type: "integer", nullable: false, defaultValue: -1),
                    SubscriptionsGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCode", x => x.PromoCode);
                });

            migrationBuilder.CreateTable(
                name: "AppliedPromoCodes",
                columns: table => new
                {
                    PromoCode = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedPromoCodes", x => new { x.PromoCode, x.UserName });
                    table.ForeignKey(
                        name: "FK_AppliedPromoCodes_PromoCode_PromoCode",
                        column: x => x.PromoCode,
                        principalTable: "PromoCode",
                        principalColumn: "PromoCode",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedPromoCodes");

            migrationBuilder.DropTable(
                name: "PromoCode");
        }
    }
}
