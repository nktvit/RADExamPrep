using ConsoleApp.Queries;

namespace ConsoleApp;

using ClassLibrary;
using ClassLibrary.Services;
using ConsoleApp.Queries;

class Program
{
    static async Task Main(string[] args)
    {
        var context = new FlightContext();
        var queries = new FlightQueries(context);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nFlight Management System");
            Console.WriteLine("1. List Passengers for Flight");
            Console.WriteLine("2. List Flight Revenue");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Enter Flight ID: ");
                    if (int.TryParse(Console.ReadLine(), out int flightId))
                    {
                        await queries.list_passengers(flightId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Flight ID");
                    }
                    break;

                case "2":
                    await queries.list_flight_revenue();
                    break;

                case "3":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}