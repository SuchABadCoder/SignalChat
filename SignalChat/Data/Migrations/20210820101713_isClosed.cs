using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalChat.Data.Migrations
{
    public partial class isClosed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isClosed",
                table: "Chats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "Chats");
        }
    }
}
