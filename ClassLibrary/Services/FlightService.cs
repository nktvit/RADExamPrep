using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Services;

public class FlightService
{
    private readonly FlightContext _context;

    public FlightService(FlightContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<dynamic>> GetPassengersForFlight(int flightId)
    {
        return await _context.PassengerBookings
            .Where(pb => pb.FlightID == flightId)
            .Select(pb => new
            {
                PassengerName = pb.Passenger.Name,
                TicketType = pb.TicketType,
                Destination = pb.Flight.Destination
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<dynamic>> GetFlightRevenues()
    {
        return await _context.Flights
            .Select(f => new
            {
                FlightNumber = f.FlightNumber,
                Destination = f.Destination,
                DepartureDate = f.DepartureDate,
                Revenue = f.PassengerBookings.Sum(pb => pb.TicketCost + pb.BaggageCharge)
            })
            .ToListAsync();
    }
}