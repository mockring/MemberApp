using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemerApp.Migrations
{
    /// <inheritdoc />
    public partial class IDName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Members",
                newName: "MemberName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Members",
                newName: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MemberName",
                table: "Members",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Members",
                newName: "Id");
        }
    }
}
