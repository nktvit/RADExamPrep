using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Models;

public class Passenger
{
    [Key]
    public int PassengerID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(7)] // As per exam constraint
    public string PassportNumber { get; set; }

    // Navigation property
    public virtual ICollection<PassengerBooking> PassengerBookings { get; set; }
}