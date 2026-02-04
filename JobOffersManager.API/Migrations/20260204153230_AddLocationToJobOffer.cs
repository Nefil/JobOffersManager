using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOffersManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToJobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "JobOffers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "JobOffers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "JobOffers");
        }
    }
}
