using System.ComponentModel.DataAnnotations;
using ClassLibrary.Enums;

namespace ClassLibrary.Models;

public class PassengerBooking
{
    [Key]
    public int PassengerBookingID { get; set; }

    [Required]
    public int PassengerID { get; set; }

    [Required]
    public int FlightID { get; set; }

    [Required]
    public TicketType TicketType { get; set; }

    [Required]
    [Range(5.01, double.MaxValue)] // As per exam constraint: "ticket cost must be more than 5 euro"
    public decimal TicketCost { get; set; }

    public decimal BaggageCharge { get; set; }

    // Navigation properties
    public virtual Passenger Passenger { get; set; }
    public virtual Flight Flight { get; set; }
}