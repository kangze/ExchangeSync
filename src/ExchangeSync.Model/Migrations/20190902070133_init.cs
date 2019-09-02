using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeSync.Model.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConnects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    UserName = table.Column<string>(maxLength: 64, nullable: true),
                    Number = table.Column<string>(maxLength: 32, nullable: true),
                    IdCard = table.Column<string>(maxLength: 64, nullable: true),
                    HashPassword = table.Column<string>(maxLength: 256, nullable: true),
                    OpenId = table.Column<string>(maxLength: 256, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWeChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    SsoId = table.Column<string>(nullable: true),
                    OpenId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeChats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConnects");

            migrationBuilder.DropTable(
                name: "UserWeChats");
        }
    }
}
