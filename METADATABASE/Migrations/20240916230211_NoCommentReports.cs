using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace METADATABASE.Migrations
{
    /// <inheritdoc />
    public partial class NoCommentReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_CommentsID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CommentsID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CommentsID",
                table: "Reports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsID",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CommentsID",
                table: "Reports",
                column: "CommentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_CommentsID",
                table: "Reports",
                column: "CommentsID",
                principalTable: "Comments",
                principalColumn: "CommentsID");
        }
    }
}
