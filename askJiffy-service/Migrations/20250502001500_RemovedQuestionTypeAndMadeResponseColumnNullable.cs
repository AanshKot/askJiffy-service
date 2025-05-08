using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace askJiffy_service.Migrations
{
    /// <inheritdoc />
    public partial class RemovedQuestionTypeAndMadeResponseColumnNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<string>(
                name: "Response",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Response",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionType",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
