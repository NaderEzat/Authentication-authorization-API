using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    public partial class intial1231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUser_AspNetUsers_Id",
                table: "SchoolUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolUser",
                table: "SchoolUser");

            migrationBuilder.RenameTable(
                name: "SchoolUser",
                newName: "SchoolUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolUsers",
                table: "SchoolUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_Id",
                table: "SchoolUsers",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_Id",
                table: "SchoolUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolUsers",
                table: "SchoolUsers");

            migrationBuilder.RenameTable(
                name: "SchoolUsers",
                newName: "SchoolUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolUser",
                table: "SchoolUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUser_AspNetUsers_Id",
                table: "SchoolUser",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
