using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExchangeSync.Model.Migrations
{
    public partial class addnewMailEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewMailEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NewMailId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TextBody = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Notify = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewMailEvents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewMailEvents");
        }
    }
}
