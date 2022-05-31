using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class AddNotificationTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTokens",
                columns: table => new
                {
                    Token = table.Column<string>(maxLength: 178, nullable: false),
                    PlatformKind = table.Column<int>(nullable: false),
                    DeviceName = table.Column<string>(maxLength: 30, nullable: true),
                    UserId = table.Column<string>(maxLength: 36, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTokens", x => x.Token);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTokens");
        }
    }
}
