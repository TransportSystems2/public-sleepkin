using Microsoft.EntityFrameworkCore.Migrations;

namespace Pillow.Infrastructure.Data.Migrations
{
    public partial class TitleAddToTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Tracks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Tracks");
        }
    }
}
