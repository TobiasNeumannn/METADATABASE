using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace METADATABASE.Migrations
{
    /// <inheritdoc />
    public partial class UserPfp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pfp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjThumbnailImg",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "pfpName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "thumbName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pfpName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "thumbName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Pfp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjThumbnailImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
