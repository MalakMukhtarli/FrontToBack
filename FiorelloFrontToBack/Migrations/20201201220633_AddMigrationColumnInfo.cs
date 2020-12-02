using Microsoft.EntityFrameworkCore.Migrations;

namespace FiorelloFrontToBack.Migrations
{
    public partial class AddMigrationColumnInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Experts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info",
                table: "Experts");
        }
    }
}
