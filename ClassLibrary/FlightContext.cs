using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary;

public sealed class FlightContext : DbContext
{
    public FlightContext(DbContextOptions<FlightContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<PassengerBooking> PassengerBookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Only use this if not configured in startup
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=flights.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}