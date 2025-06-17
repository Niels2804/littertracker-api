using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trashtracker_api.Migrations
{
    /// <inheritdoc />
    public partial class litterinitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LitterId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TemperatureCelsius = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Litter",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false),
                    Confidence = table.Column<float>(type: "real", nullable: false),
                    LocationLongitude = table.Column<float>(type: "real", nullable: false),
                    LocationLatitude = table.Column<float>(type: "real", nullable: false),
                    DetectionTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Litter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Litter_WeatherInfo_Id",
                        column: x => x.Id,
                        principalTable: "WeatherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WeatherInfo",
                columns: new[] { "Id", "Conditions", "Humidity", "TemperatureCelsius" },
                values: new object[] { "87EC8C26-0E4F-403A-94A5-9B1AC13710AE", "Clear", 50f, 20f });

            migrationBuilder.InsertData(
                table: "Litter",
                columns: new[] { "Id", "Classification", "Confidence", "DetectionTime", "LocationLatitude", "LocationLongitude" },
                values: new object[] { "87EC8C26-0E4F-403A-94A5-9B1AC13710AE", 1, 0.95f, new DateTime(2025, 6, 17, 14, 23, 45, 123, DateTimeKind.Unspecified), 37.7749f, -122.4194f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteLocations");

            migrationBuilder.DropTable(
                name: "Litter");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeatherInfo");
        }
    }
}
