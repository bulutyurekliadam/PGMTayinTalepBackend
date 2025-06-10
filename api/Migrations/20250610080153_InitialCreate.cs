using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TayinTalepAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SicilNo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Ad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Unvan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MevcutAdliye = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IseBaslamaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_SicilNo", x => x.SicilNo);
                });

            migrationBuilder.CreateTable(
                name: "TayinTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SicilNo = table.Column<string>(type: "TEXT", nullable: false),
                    TalepEdilenAdliye = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BasvuruTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: false),
                    TalepDurumu = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DegerlendirilmeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DegerlendirmeNotu = table.Column<string>(type: "TEXT", nullable: true),
                    IsOnaylandi = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TayinTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TayinTalepleri_Users_SicilNo",
                        column: x => x.SicilNo,
                        principalTable: "Users",
                        principalColumn: "SicilNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TayinTalepleri_SicilNo",
                table: "TayinTalepleri",
                column: "SicilNo");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SicilNo",
                table: "Users",
                column: "SicilNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TayinTalepleri");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
