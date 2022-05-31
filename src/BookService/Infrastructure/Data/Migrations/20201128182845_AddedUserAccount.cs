using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class AddedUserAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Likes",
                table: "Books",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookCode = table.Column<string>(type: "text", nullable: false),
                    UserAccountUserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteBooks_Books_BookCode",
                        column: x => x.BookCode,
                        principalTable: "Books",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteBooks_UserAccounts_UserAccountUserName",
                        column: x => x.UserAccountUserName,
                        principalTable: "UserAccounts",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_BookCode",
                table: "FavoriteBooks",
                column: "BookCode");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_UserAccountUserName",
                table: "FavoriteBooks",
                column: "UserAccountUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteBooks");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Books");
        }
    }
}
