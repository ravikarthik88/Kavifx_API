using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kavifx_API.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "Profiles",
                newName: "PictureMimeType");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Profiles",
                newName: "ProfilePicId");

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureData",
                table: "Profiles",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserProfileId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PictureData",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "PictureMimeType",
                table: "Profiles",
                newName: "PictureUrl");

            migrationBuilder.RenameColumn(
                name: "ProfilePicId",
                table: "Profiles",
                newName: "UserProfileId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "Profiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
