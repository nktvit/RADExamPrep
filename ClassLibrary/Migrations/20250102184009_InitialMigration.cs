using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightID);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    PassengerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PassportNumber = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.PassengerID);
                });

            migrationBuilder.CreateTable(
                name: "PassengerBookings",
                columns: table => new
                {
                    PassengerBookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerID = table.Column<int>(type: "int", nullable: false),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    TicketType = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaggageCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerBookings", x => x.PassengerBookingID);
                    table.ForeignKey(
                        name: "FK_PassengerBookings_Flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "Flights",
                        principalColumn: "FlightID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerBookings_Passengers_PassengerID",
                        column: x => x.PassengerID,
                        principalTable: "Passengers",
                        principalColumn: "PassengerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "FlightID", "Country", "DepartureDate", "Destination", "FlightNumber", "MaxSeats", "Origin" },
                values: new object[,]
                {
                    { 1, "Italy", new DateTime(2025, 1, 12, 22, 0, 0, 0, DateTimeKind.Unspecified), "Rome", "IT-001", 110, "Dublin" },
                    { 2, "England", new DateTime(2025, 1, 12, 22, 0, 0, 0, DateTimeKind.Unspecified), "London", "EN-002", 110, "Dublin" }
                });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "PassengerID", "Name", "PassportNumber" },
                values: new object[] { 1, "Fred Farnell", "P010203" });

            migrationBuilder.InsertData(
                table: "PassengerBookings",
                columns: new[] { "PassengerBookingID", "BaggageCharge", "FlightID", "PassengerID", "TicketCost", "TicketType" },
                values: new object[] { 1, 30m, 1, 1, 51.83m, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookings_FlightID",
                table: "PassengerBookings",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerBookings_PassengerID",
                table: "PassengerBookings",
                column: "PassengerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassengerBookings");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Passengers");
        }
    }
}
