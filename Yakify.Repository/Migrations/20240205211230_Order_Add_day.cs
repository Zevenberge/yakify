using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yakify.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Order_Add_day : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Order");
        }
    }
}
