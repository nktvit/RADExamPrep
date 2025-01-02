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

    public class PassengerDetailsDTO
    {
        public string PassengerName { get; set; }
        public string TicketType { get; set; }
        public string Destination { get; set; }
    }
    public class FlightRevenueDTO
    {
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Revenue { get; set; }
    }

    public async Task<IEnumerable<PassengerDetailsDTO>> GetPassengersForFlight(int flightId)
    {
        var passengers = await _context.PassengerBookings
            .Include(pb => pb.Passenger)
            .Include(pb => pb.Flight)
            .Where(pb => pb.FlightID == flightId)
            .Select(pb => new PassengerDetailsDTO
            {
                PassengerName = pb.Passenger.Name,
                TicketType = pb.TicketType.ToString(),
                Destination = pb.Flight.Destination
            })
            .ToListAsync();

        return passengers;
    }

    public async Task<IEnumerable<FlightRevenueDTO>> GetFlightRevenues()
    {
        var flights = await _context.Flights.ToListAsync();
        var bookings = await _context.PassengerBookings.ToListAsync();

        return flights.Select(f => new FlightRevenueDTO
        {
            FlightNumber = f.FlightNumber,
            Destination = f.Destination,
            DepartureDate = f.DepartureDate,
            Revenue = bookings
                .Where(b => b.FlightID == f.FlightID)
                .Sum(b => b.TicketCost + b.BaggageCharge)
        });
    }
}