using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitNews.Migrations
{
    public partial class CreatedComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Comments");
        }
    }
}
