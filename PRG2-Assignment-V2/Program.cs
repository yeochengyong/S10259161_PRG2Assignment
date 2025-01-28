using PRG2_Assignment_V2;

var airlines = new Dictionary<string, Airline>();
var boardingGates = new Dictionary<string, BoardingGate>();
var flights = new Dictionary<string, Flight>();

// new terminal
Terminal terminal = new Terminal("Changi Airport Terminal 5");

// loading airlines
Console.WriteLine("Loading Airlines...");
using (var lines = new StreamReader("airlines.csv"))
{
    string headerLine = lines.ReadLine(); // skip header
    string line;
    while ((line = lines.ReadLine()) != null)
    {
        var parts = line.Split(',');
        if (parts.Length >= 2)
        {
            string code = parts[0].Trim();
            string name = parts[1].Trim();
            airlines[code] = new Airline(code, name);
        }
    }
}
Console.WriteLine($"{airlines.Count} Airlines Loaded!");

// loading boarding gates
Console.WriteLine("Loading boarding gates...");
using (var lines = new StreamReader("boardinggates.csv"))
{
    string headerLine = lines.ReadLine();
    string line;
    while ((line = lines.ReadLine()) != null)
    {
        var parts = line.Split(',');
        if (parts.Length >= 4)
        {
            string gateName = parts[0].Trim();
            bool supportsCFFT = parts[1].Trim().ToLower() == "true";
            bool supportsDDJB = parts[2].Trim().ToLower() == "true";
            bool supportsLWTT = parts[3].Trim().ToLower() == "true";
            boardingGates[gateName] = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
        }
    }
}

Console.WriteLine($"{boardingGates.Count} Boarding Gates Loaded!");

// loading flights
Console.WriteLine("Loading flights...");
using (var reader = new StreamReader("flights.csv"))
{
    string headerLine = reader.ReadLine(); // Skip header
    string line;

    while ((line = reader.ReadLine()) != null)
    {
        var parts = line.Split(',');

        // Ensure the line has at least 4 fields (Flight Number, Origin, Destination, Expected Departure/Arrival)
        if (parts.Length >= 4)
        {
            string flightNumber = parts[0].Trim();
            string origin = parts[1].Trim();
            string destination = parts[2].Trim();
            DateTime expectedTime;

            // Validate and parse the Expected Departure/Arrival field
            if (DateTime.TryParse(parts[3].Trim(), out expectedTime))
            {
                string specialRequestCode = parts.Length > 4 ? parts[4].Trim().ToUpper() : null;

                // Determine the flight type based on the special request code
                Flight flight;
                switch (specialRequestCode)
                {
                    case "CFFT":
                        flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                        break;
                    case "LWTT":
                        flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                        break;
                    case "DDJB":
                        flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                        break;
                    default:
                        flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "Scheduled");
                        break;
                }

                // Add the flight to the dictionary
                if (!terminal.Flights.ContainsKey(flightNumber))
                {
                    terminal.Flights[flightNumber] = flight;
                }
            }
        }
    }
}

while (true)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine($"Welcome to {terminal.TerminalName}");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("0. Exit");
    Console.WriteLine("=============================================");
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            terminal.ListAllFlights();
            break;
        case "0":
            Console.WriteLine("Thank you for using the system. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }

    Console.WriteLine("\nPress Enter to return to the menu...");
    Console.ReadLine();
}