using Microsoft.EntityFrameworkCore.Migrations;

namespace PollyTest1.Migrations
{
    public partial class DBV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JJInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FSRQ = table.Column<string>(nullable: true),
                    DWJZ = table.Column<string>(nullable: true),
                    LJJZ = table.Column<string>(nullable: true),
                    SDATE = table.Column<string>(nullable: true),
                    ACTUALSYI = table.Column<string>(nullable: true),
                    NAVTYPE = table.Column<string>(nullable: true),
                    JZZZL = table.Column<string>(nullable: true),
                    SGZT = table.Column<string>(nullable: true),
                    SHZT = table.Column<string>(nullable: true),
                    FHFCZ = table.Column<string>(nullable: true),
                    FHFCBZ = table.Column<string>(nullable: true),
                    DTYPE = table.Column<string>(nullable: true),
                    FHSP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JJInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JJInfo");
        }
    }
}
