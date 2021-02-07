using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cities.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(maxLength: 256, nullable: false),
                    ToruistRating = table.Column<int>(nullable: false),
                    DateEstablished = table.Column<DateTime>(nullable: false),
                    Population = table.Column<int>(nullable: false),
                    Alpha2Code = table.Column<string>(maxLength: 2, nullable: false),
                    Alpha3Code = table.Column<string>(maxLength: 3, nullable: false),
                    CurrenciesCode = table.Column<string>(maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
