using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PlexLander.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Icon = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlexSessions",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    SessionStart = table.Column<DateTime>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlexSessions", x => new { x.Email, x.Token });
                });

            migrationBuilder.CreateTable(
                name: "PlexServer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Endpoint = table.Column<string>(nullable: true),
                    Hostname = table.Column<string>(nullable: true),
                    PlexAuthenticationEmail = table.Column<string>(nullable: true),
                    PlexAuthenticationToken = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlexServer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlexServer_PlexSessions_PlexAuthenticationEmail_PlexAuthenticationToken",
                        columns: x => new { x.PlexAuthenticationEmail, x.PlexAuthenticationToken },
                        principalTable: "PlexSessions",
                        principalColumns: new[] { "Email", "Token" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlexServer_PlexAuthenticationEmail_PlexAuthenticationToken",
                table: "PlexServer",
                columns: new[] { "PlexAuthenticationEmail", "PlexAuthenticationToken" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "PlexServer");

            migrationBuilder.DropTable(
                name: "PlexSessions");
        }
    }
}
