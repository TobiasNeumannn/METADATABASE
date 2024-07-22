using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace METADATABASE.Migrations
{
    /// <inheritdoc />
    public partial class NullablePostsId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostsID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "PostsID",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostsID",
                table: "Comments",
                column: "PostsID",
                principalTable: "Posts",
                principalColumn: "PostsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostsID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "PostsID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostsID",
                table: "Comments",
                column: "PostsID",
                principalTable: "Posts",
                principalColumn: "PostsID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
