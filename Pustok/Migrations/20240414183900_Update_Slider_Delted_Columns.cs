using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustok.Migrations
{
    public partial class Update_Slider_Delted_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "HeaderSlider");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "HeaderSlider");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HeaderSlider");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "HeaderSlider");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "HeaderSlider",
                type: "nvarchar(700)",
                maxLength: 700,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "HeaderSlider",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HeaderSlider",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "HeaderSlider",
                type: "decimal",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
