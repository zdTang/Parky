using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkyAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureToNationalParkModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "NationalParks",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "NationalParks");
        }
    }
}
