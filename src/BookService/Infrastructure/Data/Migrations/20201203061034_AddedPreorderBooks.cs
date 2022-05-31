using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class AddedPreorderBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Preorders",
                table: "Books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PreorderBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookCode = table.Column<string>(type: "text", nullable: false),
                    UserAccountUserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreorderBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreorderBook_Books_BookCode",
                        column: x => x.BookCode,
                        principalTable: "Books",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreorderBook_UserAccounts_UserAccountUserName",
                        column: x => x.UserAccountUserName,
                        principalTable: "UserAccounts",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreorderBook_BookCode",
                table: "PreorderBook",
                column: "BookCode");

            migrationBuilder.CreateIndex(
                name: "IX_PreorderBook_UserAccountUserName",
                table: "PreorderBook",
                column: "UserAccountUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreorderBook");

            migrationBuilder.DropColumn(
                name: "Preorders",
                table: "Books");
        }
    }
}
