using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspSGkVk2.Migrations
{
    public partial class v13_booker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookBot",
                columns: table => new
                {
                    IdWord = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answers = table.Column<string>(type: "TEXT", nullable: true),
                    Word = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBot", x => x.IdWord);
                });
        }
    }
}
