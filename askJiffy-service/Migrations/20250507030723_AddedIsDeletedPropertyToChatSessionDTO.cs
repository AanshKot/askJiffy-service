using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace askJiffy_service.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeletedPropertyToChatSessionDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChatSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChatSessions");
        }
    }
}
