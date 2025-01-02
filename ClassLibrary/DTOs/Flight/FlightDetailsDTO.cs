namespace ClassLibrary.DTOs.Flight;

public class FlightDetailsDTO
{
    public int FlightID { get; set; }
    public string FlightNumber { get; set; }
    public DateTime DepartureDate { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Country { get; set; }
    public int MaxSeats { get; set; }
    public int AvailableSeats { get; set; }
    public List<PassengerDetailsDTO> Passengers { get; set; }
}