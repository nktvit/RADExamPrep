using ClassLibrary.DTOs.Passenger;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Services;

public class PassengerService
{
    private readonly FlightContext _context;

    public PassengerService(FlightContext context)
    {
        _context = context;
    }

    public async Task<Passenger?> CreatePassenger(string name, string passportNumber)
    {
        var passenger = new Passenger
        {
            Name = name,
            PassportNumber = passportNumber
        };

        _context.Passengers.Add(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }

    public async Task<IEnumerable<Passenger>> GetAllPassengers()
    {
        return await _context.Passengers
            .Include(p => p.PassengerBookings)
            .ToListAsync();
    }

    public async Task<Passenger?> GetPassenger(int id)
    {
        return await _context.Passengers
            .Include(p => p.PassengerBookings)
            .ThenInclude(pb => pb.Flight)
            .FirstOrDefaultAsync(p => p.PassengerID == id);
    }

    public async Task<bool> UpdatePassenger(int id, string name, string passportNumber)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        if (passenger == null) return false;

        passenger.Name = name;
        passenger.PassportNumber = passportNumber;

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

    public async Task<bool> DeletePassenger(int id)
    {
        var passenger = await _context.Passengers
            .Include(p => p.PassengerBookings)
            .FirstOrDefaultAsync(p => p.PassengerID == id);

        if (passenger == null) return false;

        if (passenger.PassengerBookings.Any())
        {
            return false;
        }

        _context.Passengers.Remove(passenger);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PassengerFlightDTO>> GetPassengerFlights(int passengerId)
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

    public async Task<PassengerBookingSummaryDTO> GetPassengerSummary(int passengerId)
    {
        var bookings = await _context.PassengerBookings
            .Include(pb => pb.Flight)
            .Where(pb => pb.PassengerID == passengerId)
            .ToListAsync();

        return new PassengerBookingSummaryDTO
        {
            TotalFlights = bookings.Count,
            TotalSpent = bookings.Sum(b => b.TicketCost + b.BaggageCharge),
            LastBooking = bookings
                .OrderByDescending(b => b.Flight.DepartureDate)
                .FirstOrDefault()?.Flight.DepartureDate
        };
    }

    public async Task<decimal> GetTotalSpent(int passengerId)
    {
        return await _context.PassengerBookings
            .Where(pb => pb.PassengerID == passengerId)
            .SumAsync(pb => pb.TicketCost + pb.BaggageCharge);
    }

    public async Task<bool> IsPassportNumberUnique(string passportNumber, int? excludePassengerId = null)
    {
        var query = _context.Passengers.AsQueryable();

        if (excludePassengerId.HasValue)
        {
            query = query.Where(p => p.PassengerID != excludePassengerId.Value);
        }

        return !await query.AnyAsync(p => p.PassportNumber == passportNumber);
    }

    public async Task<bool> HasExistingBookings(int passengerId)
    {
        return await _context.PassengerBookings
            .AnyAsync(pb => pb.PassengerID == passengerId);
    }
}