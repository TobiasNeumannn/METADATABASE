using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace METADATABASE.Migrations
{
    /// <inheritdoc />
    public partial class NoCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_CommentsID",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CommentsID",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CommentsID",
                table: "Likes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsID",
                table: "Likes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentsID",
                table: "Likes",
                column: "CommentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_CommentsID",
                table: "Likes",
                column: "CommentsID",
                principalTable: "Comments",
                principalColumn: "CommentsID");
        }
    }
}
