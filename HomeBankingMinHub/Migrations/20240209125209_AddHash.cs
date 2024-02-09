using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMinHub.Migrations
{
    /// <inheritdoc />
    public partial class AddHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Clients");
        }
    }
}
