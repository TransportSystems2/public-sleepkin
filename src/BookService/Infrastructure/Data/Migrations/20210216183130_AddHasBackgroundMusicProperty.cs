using Microsoft.EntityFrameworkCore.Migrations;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class AddHasBackgroundMusicProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBackgroundMusic",
                table: "Tracks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBackgroundMusic",
                table: "Tracks");
        }
    }
}
