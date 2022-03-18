using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoPlayServer.Migrations
{
    public partial class AdminActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "banned",
                table: "AppUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "mutedFor",
                table: "AppUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "mutedOn",
                table: "AppUsers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banned",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "mutedFor",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "mutedOn",
                table: "AppUsers");
        }
    }
}
