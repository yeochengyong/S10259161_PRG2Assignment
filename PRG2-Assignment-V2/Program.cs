using PRG2_Assignment_V2;

// new terminal
Terminal terminal = new Terminal("Changi Airport Terminal 5");

// loading airlines
Console.WriteLine("Loading Airlines...");
using (var lines = new StreamReader("airlines.csv"))
{
    string headerLine = lines.ReadLine();
    string line;
    while ((line = lines.ReadLine()) != null)
    {
        var parts = line.Split(',');
        if (parts.Length >= 2)
        {
            string code = parts[0].Trim();
            string name = parts[1].Trim();
            Airline airline = new Airline(code, name);
            terminal.Airlines[code] = airline;
        }
    }
}
Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!");

// loading boarding gates
Console.WriteLine("Loading boarding gates...");
using (var reader = new StreamReader("boardinggates.csv"))
{
    string headerLine = reader.ReadLine();
    string line;

    while ((line = reader.ReadLine()) != null)
    {
        var parts = line.Split(',');
        if (parts.Length >= 4)
        {
            string gateName = parts[0].Trim();
            bool supportsDDJB = parts[1].Trim().ToLower() == "true";
            bool supportsCFFT = parts[2].Trim().ToLower() == "true";
            bool supportsLWTT = parts[3].Trim().ToLower() == "true";

            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
            terminal.BoardingGates[gateName] = gate;
        }
    }
}
Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!");

// loading flights
Console.WriteLine("Loading flights...");
using (var reader = new StreamReader("flights.csv"))
{
    string headerLine = reader.ReadLine();
    string line;

    while ((line = reader.ReadLine()) != null)
    {
        var parts = line.Split(',');

        // ensure the line has at least 4 fields
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

                // determine flight type base on special req code
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

                // add flight to dictionary
                if (!terminal.Flights.ContainsKey(flightNumber))
                {
                    terminal.Flights[flightNumber] = flight;
                }
            }
        }
    }
}
Console.WriteLine($"{terminal.Flights.Count} Flights Loaded!");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=============================================");
    Console.WriteLine($"Welcome to {terminal.TerminalName}");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.WriteLine("=============================================");
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            terminal.ListAllFlights();
            break;
        case "2":
            terminal.ListAllBoardingGates();
            break;
        case "3":
            terminal.AssignBoardingGateToFlight();
            break;
        case "0":
            Console.WriteLine("Thank you for using the system. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}