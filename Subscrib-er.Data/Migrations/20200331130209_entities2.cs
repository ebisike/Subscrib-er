using Microsoft.EntityFrameworkCore.Migrations;

namespace Subscrib_er.Data.Migrations
{
    public partial class entities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "packages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
