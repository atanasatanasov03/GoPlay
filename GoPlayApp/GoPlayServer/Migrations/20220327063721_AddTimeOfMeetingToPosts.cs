using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoPlayServer.Migrations
{
    public partial class AddTimeOfMeetingToPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "timeOfMeeting",
                table: "Posts",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeOfMeeting",
                table: "Posts");
        }
    }
}
