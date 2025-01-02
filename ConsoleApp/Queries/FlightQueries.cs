using ClassLibrary;
using ClassLibrary.Services;
using ConsoleTables;

namespace ConsoleApp.Queries;

public class FlightQueries
{
    private readonly FlightContext _context;
    private readonly FlightService flightService;

    public FlightQueries(FlightContext context)
    {
        _context = context;
        flightService = new FlightService(context);
    }

    public async void list_passengers(int FlightID)
    {
        var passengers = await flightService.GetPassengersForFlight(FlightID);

        var table = new ConsoleTable("Name", "Ticket Type", "Destination");

        foreach (var passenger in passengers)
        {
            table.AddRow(
                passenger.PassengerName,
                passenger.TicketType,
                passenger.Destination
            );
        }

        Console.WriteLine($"\nPassenger List for Flight {FlightID}");
        table.Write(Format.Alternative);
    }

    public async void list_flight_revenue()
    {
        var revenues = await flightService.GetFlightRevenues();

        var table = new ConsoleTable("Flight", "Destination", "Departure", "Revenue");

        foreach (var revenue in revenues)
        {
            table.AddRow(
                revenue.FlightNumber,
                revenue.Destination,
                revenue.DepartureDate.ToString("dd/MM/yyyy HH:mm"),
                $"€{revenue.Revenue:F2}"
            );
        }

        Console.WriteLine("\nFlight Revenue Report");
        table.Write(Format.Alternative);
    }
}