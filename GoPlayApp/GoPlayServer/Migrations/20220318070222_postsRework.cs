using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoPlayServer.Migrations
{
    public partial class postsRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsPosts");

            migrationBuilder.DropTable(
                name: "PlayPosts");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false),
                    heading = table.Column<string>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    timeOfCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    play = table.Column<bool>(type: "INTEGER", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: true),
                    groupId = table.Column<Guid>(type: "TEXT", nullable: true),
                    pictureUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Groups_groupId",
                        column: x => x.groupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_groupId",
                table: "Posts",
                column: "groupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.CreateTable(
                name: "NewsPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    heading = table.Column<string>(type: "TEXT", nullable: false),
                    pictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    timeOfCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    groupName = table.Column<string>(type: "TEXT", nullable: false),
                    heading = table.Column<string>(type: "TEXT", nullable: false),
                    timeOfCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    userId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayPosts", x => x.Id);
                });
        }
    }
}
