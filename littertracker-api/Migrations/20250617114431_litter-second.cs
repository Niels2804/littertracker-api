using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trashtracker_api.Migrations
{
    /// <inheritdoc />
    public partial class littersecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Litter_WeatherInfo_Id",
                table: "Litter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Litter",
                table: "Litter");

            migrationBuilder.RenameTable(
                name: "Litter",
                newName: "Litters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Litters",
                table: "Litters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_WeatherInfo_Id",
                table: "Litters",
                column: "Id",
                principalTable: "WeatherInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Litters_WeatherInfo_Id",
                table: "Litters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Litters",
                table: "Litters");

            migrationBuilder.RenameTable(
                name: "Litters",
                newName: "Litter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Litter",
                table: "Litter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Litter_WeatherInfo_Id",
                table: "Litter",
                column: "Id",
                principalTable: "WeatherInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
