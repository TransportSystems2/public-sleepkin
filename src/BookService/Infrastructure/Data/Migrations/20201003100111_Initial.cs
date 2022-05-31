using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    CoverUri = table.Column<string>(nullable: true),
                    AccessLevel = table.Column<int>(nullable: false),
                    Author = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    BookCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Format = table.Column<string>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    IsTrailer = table.Column<bool>(nullable: false, defaultValue: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Tracks_Books_BookCode",
                        column: x => x.BookCode,
                        principalTable: "Books",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookTag",
                columns: table => new
                {
                    BookCode = table.Column<string>(nullable: false),
                    TagCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTag", x => new { x.BookCode, x.TagCode });
                    table.ForeignKey(
                        name: "FK_BookTag_Books_BookCode",
                        column: x => x.BookCode,
                        principalTable: "Books",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTag_Tags_TagCode",
                        column: x => x.TagCode,
                        principalTable: "Tags",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookTag_TagCode",
                table: "BookTag",
                column: "TagCode");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_BookCode",
                table: "Tracks",
                column: "BookCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookTag");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
