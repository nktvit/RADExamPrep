﻿// <auto-generated />
using System;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClassLibrary.Migrations
{
    [DbContext(typeof(FlightContext))]
    partial class FlightContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassLibrary.Models.Flight", b =>
                {
                    b.Property<int>("FlightID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlightID"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("MaxSeats")
                        .HasColumnType("int");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("FlightID");

                    b.ToTable("Flights");

                    b.HasData(
                        new
                        {
                            FlightID = 1,
                            Country = "Italy",
                            DepartureDate = new DateTime(2025, 1, 12, 22, 0, 0, 0, DateTimeKind.Unspecified),
                            Destination = "Rome",
                            FlightNumber = "IT-001",
                            MaxSeats = 110,
                            Origin = "Dublin"
                        },
                        new
                        {
                            FlightID = 2,
                            Country = "England",
                            DepartureDate = new DateTime(2025, 1, 12, 22, 0, 0, 0, DateTimeKind.Unspecified),
                            Destination = "London",
                            FlightNumber = "EN-002",
                            MaxSeats = 110,
                            Origin = "Dublin"
                        });
                });

            modelBuilder.Entity("ClassLibrary.Models.Passenger", b =>
                {
                    b.Property<int>("PassengerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.HasKey("PassengerID");

                    b.ToTable("Passengers");

                    b.HasData(
                        new
                        {
                            PassengerID = 1,
                            Name = "Fred Farnell",
                            PassportNumber = "P010203"
                        });
                });

            modelBuilder.Entity("ClassLibrary.Models.PassengerBooking", b =>
                {
                    b.Property<int>("PassengerBookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PassengerBookingID"));

                    b.Property<decimal>("BaggageCharge")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("FlightID")
                        .HasColumnType("int");

                    b.Property<int>("PassengerID")
                        .HasColumnType("int");

                    b.Property<decimal>("TicketCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TicketType")
                        .HasColumnType("int");

                    b.HasKey("PassengerBookingID");

                    b.HasIndex("FlightID");

                    b.HasIndex("PassengerID");

                    b.ToTable("PassengerBookings");

                    b.HasData(
                        new
                        {
                            PassengerBookingID = 1,
                            BaggageCharge = 30m,
                            FlightID = 1,
                            PassengerID = 1,
                            TicketCost = 51.83m,
                            TicketType = 0
                        });
                });

            modelBuilder.Entity("ClassLibrary.Models.PassengerBooking", b =>
                {
                    b.HasOne("ClassLibrary.Models.Flight", "Flight")
                        .WithMany("PassengerBookings")
                        .HasForeignKey("FlightID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary.Models.Passenger", "Passenger")
                        .WithMany("PassengerBookings")
                        .HasForeignKey("PassengerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("Passenger");
                });

            modelBuilder.Entity("ClassLibrary.Models.Flight", b =>
                {
                    b.Navigation("PassengerBookings");
                });

            modelBuilder.Entity("ClassLibrary.Models.Passenger", b =>
                {
                    b.Navigation("PassengerBookings");
                });
#pragma warning restore 612, 618
        }
    }
}
