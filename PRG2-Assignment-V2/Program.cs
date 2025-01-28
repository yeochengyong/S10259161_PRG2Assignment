using PRG2_Assignment_V2;

var airlines = new Dictionary<string, Airline>();
var boardingGates = new Dictionary<string, BoardingGate>();

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

var flights = new Dictionary<string, Flight>(); // Replace `object` with your Flight base class.

try
{
    using (var reader = new StreamReader("flights.csv"))
    {
        // Skip the header line
        string headerLine = reader.ReadLine();

        // Read and process each line
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
                DateTime expectedTime = DateTime.Parse(parts[3].Trim());
                string specialRequestCode = parts.Length > 4 ? parts[4].Trim().ToUpper() : null;

                Flight flight;

                // Determine the flight type based on the Special Request Code
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

                // Add the flight to the dictionary if it doesn't already exist
                if (!flights.ContainsKey(flightNumber))
                {
                    flights[flightNumber] = flight;
                }
                else
                {
                    Console.WriteLine($"Duplicate flight number {flightNumber} detected. Skipping.");
                }
            }
            else
            {
                Console.WriteLine($"Invalid line format: {line}");
            }
        }

        Console.WriteLine($"{flights.Count} Flights Loaded!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading flights: {ex.Message}");
}

