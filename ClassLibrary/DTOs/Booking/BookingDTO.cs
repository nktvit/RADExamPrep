using ClassLibrary.Enums;

namespace ClassLibrary.DTOs.Booking;

public class BookingDTO
{
    public int PassengerID { get; set; }
    public int FlightID { get; set; }
    public TicketType TicketType { get; set; }
    public decimal TicketCost { get; set; }
    public decimal BaggageCharge { get; set; }
}