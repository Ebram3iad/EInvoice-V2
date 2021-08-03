using Microsoft.EntityFrameworkCore.Migrations;

namespace EInvoiceInfrastructure.Migrations
{
    public partial class addUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "InvoiceHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InvoiceHeaders");
        }
    }
}
