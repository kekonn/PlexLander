using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlexLander.Migrations
{
    public partial class UpdateModelPlexServerv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "PlexServer");

            migrationBuilder.RenameColumn(
                name: "Hostname",
                table: "PlexServer",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Endpoint",
                table: "PlexServer",
                newName: "Uri");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "PlexServer",
                newName: "Endpoint");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PlexServer",
                newName: "Hostname");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "PlexServer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
