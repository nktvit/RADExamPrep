namespace ClassLibrary.DTOs.Passenger;

public class PassengerBookingSummaryDTO
{
    public int TotalFlights { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastBooking { get; set; }
}