using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDoneFromDemoTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "DemoTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "DemoTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
