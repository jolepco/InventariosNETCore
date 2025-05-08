using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Agregarindicesderelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_Code",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.CreateIndex(
                name: "IX_States_Code_Name",
                table: "States",
                columns: new[] { "Code", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name_Code",
                table: "Countries",
                columns: new[] { "Name", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Code_Name",
                table: "Cities",
                columns: new[] { "Code", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_States_Code_Name",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name_Code",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Code_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);
        }
    }
}
