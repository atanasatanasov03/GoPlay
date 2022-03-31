using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoPlayServer.Migrations
{
    public partial class validEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "validEmail",
                table: "AppUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "validEmail",
                table: "AppUsers");
        }
    }
}
