using Microsoft.EntityFrameworkCore.Migrations;

namespace COMP2084BeerStore.Data.Migrations
{
    public partial class AddProductPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Products");
        }
    }
}
