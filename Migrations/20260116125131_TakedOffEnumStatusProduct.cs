using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VardagshörnanApp.Migrations
{
    /// <inheritdoc />
    public partial class TakedOffEnumStatusProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
