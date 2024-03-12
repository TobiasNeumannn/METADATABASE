using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace METADATABASE.Migrations
{
    /// <inheritdoc />
    public partial class PFP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pfp",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pfp",
                table: "Feedback");
        }
    }
}
