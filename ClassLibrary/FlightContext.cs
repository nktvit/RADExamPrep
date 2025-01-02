using ClassLibrary.Enums;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClassLibrary;

public class FlightContext : DbContext
{
    public FlightContext(DbContextOptions<FlightContext> options)
    : base(options)
    {
    }

    public DbSet<Flight?> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<PassengerBooking> PassengerBookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var dbPath = Path.Join(path, "flightDB.db");

            options.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Flight-PassengerBooking Relationship
        modelBuilder.Entity<PassengerBooking>()
            .HasOne(pb => pb.Flight)
            .WithMany(f => f.PassengerBookings)
            .HasForeignKey(pb => pb.FlightID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Passenger-PassengerBooking Relationship
        modelBuilder.Entity<PassengerBooking>()
            .HasOne(pb => pb.Passenger)
            .WithMany(p => p.PassengerBookings)
            .HasForeignKey(pb => pb.PassengerID)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Data
        modelBuilder.Entity<Flight>().HasData(
            new Flight
            {
                FlightID = 1, FlightNumber = "IT-001", DepartureDate = DateTime.Parse("2025-01-12T22:00:00"),
                Origin = "Dublin", Destination = "Rome", Country = "Italy", MaxSeats = 110
            },
            new Flight
            {
                FlightID = 2, FlightNumber = "EN-002", DepartureDate = DateTime.Parse("2025-01-12T22:00:00"),
                Origin = "Dublin", Destination = "London", Country = "England", MaxSeats = 110
            }
        );

        modelBuilder.Entity<Passenger>().HasData(
            new Passenger { PassengerID = 1, Name = "Fred Farnell", PassportNumber = "P010203" }
        );

        modelBuilder.Entity<PassengerBooking>().HasData(
            new PassengerBooking
            {
                PassengerBookingID = 1, PassengerID = 1, FlightID = 1, TicketType = TicketType.Economy,
                TicketCost = 51.83m, BaggageCharge = 30m
            }
        );
    }
}