namespace ClassLibrary.DTOs.Flight;

public class FlightRevenueDTO
{
    public string FlightNumber { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureDate { get; set; }
    public decimal Revenue { get; set; }
}