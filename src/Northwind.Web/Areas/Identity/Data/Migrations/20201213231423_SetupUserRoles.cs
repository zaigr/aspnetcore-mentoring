using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Web.Areas.Identity.Data.Migrations
{
    public partial class SetupUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[]
                {
                    "Id",
                    "Name",
                    "NormalizedName"
                },
                values: new object[]
                {
                    "1",
                    "Administrator",
                    "Administrator",
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[]
                {
                    "Id",
                    "Name",
                    "NormalizedName"
                },
                values: new object[]
                {
                    "2",
                    "Standard",
                    "Standard",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
