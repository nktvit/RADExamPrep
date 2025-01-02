using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models;

public class Flight
{
    [Key]
    public int FlightID { get; set; }

    [Required]
    [StringLength(10)]
    public string FlightNumber { get; set; }

    [Required]
    public DateTime DepartureDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Origin { get; set; }

    [Required]
    [StringLength(50)]
    public string Destination { get; set; }

    [Required]
    [StringLength(50)]
    public string Country { get; set; }

    [Required]
    [Range(1, 1000)]
    public int MaxSeats { get; set; }

    // Nav
    public virtual ICollection<PassengerBooking> PassengerBookings { get; set; }
}