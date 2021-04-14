using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PollyTest1.Migrations
{
    public partial class V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityTypeName = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SZCompanyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XH = table.Column<string>(nullable: true),
                    FZJG = table.Column<string>(nullable: true),
                    ZZZSH = table.Column<string>(nullable: true),
                    FZRQ = table.Column<string>(nullable: true),
                    YXQ = table.Column<string>(nullable: true),
                    ORG_CODE = table.Column<string>(nullable: true),
                    QYMC = table.Column<string>(nullable: true),
                    QYYWLX = table.Column<string>(nullable: true),
                    TYSHXYDM = table.Column<string>(nullable: true),
                    ZZDJ = table.Column<string>(nullable: true),
                    ZZLB = table.Column<string>(nullable: true),
                    ZZXL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SZCompanyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestApi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Age = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 30, nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestApi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditEntryId = table.Column<int>(nullable: false),
                    RelationName = table.Column<string>(maxLength: 255, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 255, nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntryProperty_AuditEntry_AuditEntryId",
                        column: x => x.AuditEntryId,
                        principalTable: "AuditEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryProperty_AuditEntryId",
                table: "AuditEntryProperty",
                column: "AuditEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntryProperty");

            migrationBuilder.DropTable(
                name: "SZCompanyInfo");

            migrationBuilder.DropTable(
                name: "TestApi");

            migrationBuilder.DropTable(
                name: "AuditEntry");
        }
    }
}
