namespace ClassLibrary.DTOs.Passenger;

public class PassengerFlightDTO
{
    public int FlightID { get; set; }
    public string FlightNumber { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureDate { get; set; }
    public string TicketType { get; set; }
    public decimal TotalCost { get; set; }
}