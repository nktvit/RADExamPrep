using ClassLibrary.Models;
using ClassLibrary.DTOs.Booking;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Services;

public class BookingService
{
    private readonly FlightContext _context;
    private readonly FlightService flightService;

    public BookingService(FlightContext context, FlightService flightService)
    {
        _context = context;
        flightService = flightService;
    }

    public async Task<bool> CreateBooking(BookingDTO booking)
    {
        if (!await flightService.HasAvailableSeats(booking.FlightID))
        {
            return false;
        }

        var newBooking = new PassengerBooking
        {
            PassengerID = booking.PassengerID,
            FlightID = booking.FlightID,
            TicketType = booking.TicketType,
            TicketCost = booking.TicketCost,
            BaggageCharge = booking.BaggageCharge
        };

        _context.PassengerBookings.Add(newBooking);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelBooking(int passengerId, int flightId)
    {
        var booking = await _context.PassengerBookings
            .FirstOrDefaultAsync(pb =>
                pb.PassengerID == passengerId &&
                pb.FlightID == flightId);

        if (booking == null) return false;

        _context.PassengerBookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}