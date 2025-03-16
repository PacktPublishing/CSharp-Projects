using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTrackerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsAdmin", "PasswordHash", "Salt", "Username" },
                values: new object[] { 1, true, new byte[] { 157, 181, 200, 155, 137, 142, 235, 58, 87, 195, 100, 51, 33, 84, 250, 248, 219, 174, 47, 125, 171, 145, 189, 73, 241, 83, 221, 193, 51, 119, 209, 203 }, new byte[] { 65, 100, 109, 105, 110, 85, 115, 101, 114, 85, 115, 101, 115, 65, 72, 97, 114, 100, 99, 111, 100, 101, 100, 83, 97, 108, 116, 84, 111, 80, 114, 101, 118, 101, 110, 116, 69, 70, 69, 114, 114, 111, 114, 115 }, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
