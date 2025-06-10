using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TayinTalepAPI.Migrations
{
    /// <inheritdoc />
    public partial class TalepTuruEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TalepTuru",
                table: "TayinTalepleri",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TalepTuru",
                table: "TayinTalepleri");
        }
    }
}
