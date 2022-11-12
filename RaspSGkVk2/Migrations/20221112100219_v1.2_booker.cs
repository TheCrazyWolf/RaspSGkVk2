using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspSGkVk2.Migrations
{
    public partial class v12_booker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "BookBot",
            //    columns: table => new
            //    {
            //        IdWord = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        Word = table.Column<string>(type: "TEXT", nullable: true),
            //        Answers = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BookBot", x => x.IdWord);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Settings",
            //    columns: table => new
            //    {
            //        IdSetting = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        TokenVk = table.Column<string>(type: "TEXT", nullable: true),
            //        IdGroup = table.Column<long>(type: "INTEGER", nullable: false),
            //        Timer = table.Column<int>(type: "INTEGER", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Settings", x => x.IdSetting);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tasks",
            //    columns: table => new
            //    {
            //        IdTask = table.Column<int>(type: "INTEGER", nullable: false)
            //            .Annotation("Sqlite:Autoincrement", true),
            //        TypeTask = table.Column<char>(type: "TEXT", nullable: true),
            //        PeerId = table.Column<long>(type: "INTEGER", nullable: true),
            //        Value = table.Column<string>(type: "TEXT", nullable: true),
            //        ResultText = table.Column<string>(type: "TEXT", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tasks", x => x.IdTask);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBot");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
