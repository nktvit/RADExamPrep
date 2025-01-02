using ClassLibrary.DTOs.Booking;
using ClassLibrary.DTOs.Flight;
using ClassLibrary.DTOs.Passenger;
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

    public async Task<Flight> CreateFlight(FlightDetailsDTO flightDto)
    {
        var flight = new Flight
        {
            FlightNumber = flightDto.FlightNumber,
            DepartureDate = flightDto.DepartureDate,
            Origin = flightDto.Origin,
            Destination = flightDto.Destination,
            Country = flightDto.Country,
            MaxSeats = flightDto.MaxSeats
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight;
    }

    public async Task<Flight?> GetFlight(int id)
    {
        return await _context.Flights
            .Include(f => f.PassengerBookings)
            .ThenInclude(pb => pb.Passenger)
            .FirstOrDefaultAsync(f => f.FlightID == id);
    }

    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await _context.Flights
            .Include(f => f.PassengerBookings)
            .ThenInclude(pb => pb.Passenger)
            .ToListAsync();
    }

    public async Task<FlightDetailsDTO> GetFlightDetails(int flightId)
    {
        var flight = await _context.Flights
            .Include(f => f.PassengerBookings)
            .ThenInclude(pb => pb.Passenger)
            .FirstOrDefaultAsync(f => f.FlightID == flightId);

        if (flight == null) return null;

        var passengers = await GetPassengersForFlight(flightId);

        return new FlightDetailsDTO
        {
            FlightID = flight.FlightID,
            FlightNumber = flight.FlightNumber,
            DepartureDate = flight.DepartureDate,
            Origin = flight.Origin,
            Destination = flight.Destination,
            Country = flight.Country,
            MaxSeats = flight.MaxSeats,
            AvailableSeats = flight.MaxSeats - flight.PassengerBookings.Count,
            Passengers = passengers.ToList()
        };
    }

    public async Task<IEnumerable<PassengerDetailsDTO>> GetPassengersForFlight(int flightId)
    {
        return await _context.PassengerBookings
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

    public async Task<IEnumerable<PassengerFlightDTO>> GetFlightsForPassenger(int passengerId)
    {
        return await _context.PassengerBookings
            .Include(pb => pb.Flight)
            .Where(pb => pb.PassengerID == passengerId)
            .Select(pb => new PassengerFlightDTO
            {
                FlightID = pb.FlightID,
                FlightNumber = pb.Flight.FlightNumber,
                Destination = pb.Flight.Destination,
                DepartureDate = pb.Flight.DepartureDate,
                TicketType = pb.TicketType.ToString(),
                TotalCost = pb.TicketCost + pb.BaggageCharge
            })
            .ToListAsync();
    }

    public async Task<bool> HasAvailableSeats(int flightId)
    {
        var flight = await _context.Flights
            .Include(f => f.PassengerBookings)
            .FirstOrDefaultAsync(f => f.FlightID == flightId);

        return flight != null &&
               flight.PassengerBookings.Count < flight.MaxSeats;
    }

    public async Task<bool> UpdateFlight(int id, FlightDetailsDTO flightDto)
    {
        var flight = await _context.Flights.FindAsync(id);

        if (flight == null) return false;

        flight.FlightNumber = flightDto.FlightNumber;
        flight.DepartureDate = flightDto.DepartureDate;
        flight.Origin = flightDto.Origin;
        flight.Destination = flightDto.Destination;
        flight.Country = flightDto.Country;
        flight.MaxSeats = flightDto.MaxSeats;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteFlight(int id)
    {
        var flight = await _context.Flights
            .Include(f => f.PassengerBookings)
            .FirstOrDefaultAsync(f => f.FlightID == id);

        if (flight == null) return false;

        if (flight.PassengerBookings.Any())
        {
            return false;
        }

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddPassengerToFlight(int flightId, BookingDTO bookingDto)
    {
        var flight = await _context.Flights
            .Include(f => f.PassengerBookings)
            .FirstOrDefaultAsync(f => f.FlightID == flightId);

        if (flight == null || flight.PassengerBookings.Count >= flight.MaxSeats)
        {
            return false;
        }

        var booking = new PassengerBooking
        {
            PassengerID = bookingDto.PassengerID,
            FlightID = flightId,
            TicketType = bookingDto.TicketType,
            TicketCost = bookingDto.TicketCost,
            BaggageCharge = bookingDto.BaggageCharge
        };

        _context.PassengerBookings.Add(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}