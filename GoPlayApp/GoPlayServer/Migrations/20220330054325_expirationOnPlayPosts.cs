using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoPlayServer.Migrations
{
    public partial class expirationOnPlayPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeOfMeeting",
                table: "Posts",
                newName: "expires");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expires",
                table: "Posts",
                newName: "timeOfMeeting");
        }
    }
}
