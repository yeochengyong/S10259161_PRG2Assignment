using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10259161H
// Student Name : Yeo Cheng Yong
// Partner Name : Khairi Rasyadi
//==========================================================

namespace PRG2_Assignment_V2
{
    public class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }
        public Dictionary<string, string> AirlineCodeMapping { get; set; }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>

            {
                { "ArrivingFlight", 500 },
                { "DepartingFlight", 800 },
                { "BoardingGateBaseFee", 300 },
                { "DDJBCodeRequestFee", 300 },
                { "CFFTCodeRequestFee", 150 },
                { "LWTTCodeRequestFee", 500 }
            };

            AirlineCodeMapping = new Dictionary<string, string>
            {
                { "SQ", "Singapore Airlines" },
                { "MH", "Malaysia Airlines" },
                { "JL", "Japan Airlines" },
                { "CX", "Cathay Pacific" },
                { "QF", "Qantas Airways" },
                { "TR", "AirAsia" },
                { "EK", "Emirates" },
                { "BA", "British Airways" }
            };
        }

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code)) // if airline code NOT in airlines dictionary
            {
                Airlines[airline.Code] = airline; // add airline to dict
                return true;
            }
            return false;
        }
        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates[gate.GateName] = gate;
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }
        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"{airline.Name}: ${airline.CalculateFees():0.00}");
            }
        }
        public string GetAirlineNameFromFlight(string flightNumber)
        {
            // extract airline code from flight num
            string[] parts = flightNumber.Split(' ');
            if (parts.Length > 0)
            {
                string airlineCode = parts[0].Trim();

                // check if airline exist in map
                if (AirlineCodeMapping.TryGetValue(airlineCode, out string airlineName))
                {
                    return airlineName; // return name
                }
            }

            return "Unknown Airline"; // default
        }

        public void ListAllFlights()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine($"List of Flights for {TerminalName}");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20}",
                              "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time");

            foreach (var flight in Flights.Values)
            {

                string airlineName = GetAirlineNameFromFlight(flight.FlightNumber);
 
                // Display flight details
                Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20}",
                                  flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("hh:mm tt"));
            }
        }
        public void ListAllBoardingGates()
        {
            Console.WriteLine("=========================================================");
            Console.WriteLine($"List of Boarding Gates for {TerminalName}");
            Console.WriteLine("=========================================================");
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10}",
                              "Gate Name", "DDJB", "CFFT", "LWTT");

            foreach (var gate in BoardingGates.Values)
            {
                // display gate details
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10}",
                                  gate.GateName,
                                  gate.SupportsDDJB ? "True" : "False",
                                  gate.SupportsCFFT ? "True" : "False",
                                  gate.SupportsLWTT ? "True" : "False");
            }
        }
        public void AssignBoardingGateToFlight()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");

            Flight selectedFlight = null;

            // Prompt the user for the Flight Number
            while (selectedFlight == null)
            {
                Console.WriteLine("\nEnter Flight Number: ");
                string flightNumber = Console.ReadLine().Trim();

                if (Flights.ContainsKey(flightNumber))
                {
                    selectedFlight = Flights[flightNumber];
                }
                else
                {
                    Console.WriteLine("Invalid Flight Number. Please try again.");
                }
            }

            // Prompt for a Boarding Gate
            BoardingGate selectedGate = null;
            while (selectedGate == null)
            {
                Console.WriteLine("\nEnter Boarding Gate Name: ");
                string gateName = Console.ReadLine().Trim();

                if (BoardingGates.ContainsKey(gateName))
                {
                    BoardingGate gate = BoardingGates[gateName];

                    // Check if the gate is already assigned
                    if (gate.AssignedFlight == null)
                    {
                        selectedGate = gate;
                    }
                    else
                    {
                        Console.WriteLine($"Boarding Gate {gateName} is already assigned to Flight {gate.AssignedFlight.FlightNumber}. Please choose another gate.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Boarding Gate. Please try again.");
                }
            }

            // Display Flight and Boarding Gate Information in the Correct Format
            Console.WriteLine("\nFlight Number: " + selectedFlight.FlightNumber);
            Console.WriteLine("Origin: " + selectedFlight.Origin);
            Console.WriteLine("Destination: " + selectedFlight.Destination);
            Console.WriteLine("Expected Time: " + selectedFlight.ExpectedTime.ToString("d/M/yyyy hh:mm:ss tt"));
            Console.WriteLine("Special Request Code: " + (selectedFlight is NORMFlight ? "None" : selectedFlight.GetType().Name.Replace("Flight", "")));

            Console.WriteLine("\nBoarding Gate Name: " + selectedGate.GateName);
            Console.WriteLine("Supports DDJB: " + (selectedGate.SupportsDDJB ? "True" : "False"));
            Console.WriteLine("Supports CFFT: " + (selectedGate.SupportsCFFT ? "True" : "False"));
            Console.WriteLine("Supports LWTT: " + (selectedGate.SupportsLWTT ? "True" : "False"));

            // Prompt user to update Flight Status
            Console.WriteLine("\nWould you like to update the status of the flight? (Y/N): ");
            string updateStatus = Console.ReadLine().Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.WriteLine("\n1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.WriteLine("\nPlease select the new status of the flight: ");

                int statusChoice;
                while (!int.TryParse(Console.ReadLine().Trim(), out statusChoice) || statusChoice < 1 || statusChoice > 3)
                {
                    Console.WriteLine("Invalid selection. Please select 1, 2, or 3: ");
                }

                string[] statusOptions = { "Delayed", "Boarding", "On Time" };
                selectedFlight.Status = statusOptions[statusChoice - 1]; // Assigns the correct status
            }
            else
            {
                selectedFlight.Status = "On Time"; // Default status
            }

            // Assign the Flight to the Boarding Gate
            selectedGate.AssignedFlight = selectedFlight;
            Console.WriteLine($"\nBoarding Gate {selectedGate.GateName} successfully assigned to Flight {selectedFlight.FlightNumber}.");
        }
        public void CreateFlight()
        {
            bool addMoreFlights = true;

            while (addMoreFlights)
            {
                string flightNumber = "";
                while (true)
                {
                    Console.Write("\nEnter Flight Number: ");
                    flightNumber = Console.ReadLine().Trim().ToUpper();

                    if (string.IsNullOrEmpty(flightNumber))
                    {
                        Console.WriteLine("Error: Flight Number cannot be empty. Please try again.");
                        continue;
                    }

                    if (Flights.ContainsKey(flightNumber))
                    {
                        Console.WriteLine("Error: Flight Number already exists! Please enter a unique Flight Number.");
                        continue;
                    }

                    break;
                }
                string airlineName = GetAirlineNameFromFlight(flightNumber);

                string origin = "";
                while (true)
                {
                    Console.Write("\nEnter Origin: ");
                    origin = Console.ReadLine().Trim();
                    origin = FormatLocation(origin);

                    if (string.IsNullOrEmpty(origin))
                    {
                        Console.WriteLine("Error: Origin cannot be empty. Please try again.");
                    }
                    else
                    {
                        break;
                    }
                }

                string destination = "";
                while (true)
                {
                    Console.Write("\nEnter Destination: ");
                    destination = Console.ReadLine().Trim();
                    destination = FormatLocation(destination);

                    if (string.IsNullOrEmpty(destination))
                    {
                        Console.WriteLine("Error: Destination cannot be empty. Please try again.");
                    }
                    else
                    {
                        break;
                    }
                }

                DateTime expectedTime;
                while (true)
                {
                    Console.Write("\nEnter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    string timeInput = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(timeInput) || !DateTime.TryParseExact(timeInput, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                    {
                        Console.WriteLine("Error: Invalid format! Please enter the date in dd/mm/yyyy hh:mm format.");
                    }
                    else
                    {
                        break;
                    }
                }

                // Prompt for Special Request Code
                string specialRequestCode = "";
                while (true)
                {
                    Console.Write("\nEnter Special Request Code (CFFT/DDJB/LWTT/None): ");
                    specialRequestCode = Console.ReadLine().Trim().ToUpper();

                    if (specialRequestCode == "CFFT" || specialRequestCode == "DDJB" || specialRequestCode == "LWTT" || specialRequestCode == "NONE")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid input! Please enter CFFT, DDJB, LWTT, or None.");
                    }
                }

                // Create appropriate Flight object
                Flight newFlight = specialRequestCode switch
                {
                    "CFFT" => new CFFTFlight(flightNumber, origin, destination, expectedTime, "Scheduled"),
                    "DDJB" => new DDJBFlight(flightNumber, origin, destination, expectedTime, "Scheduled"),
                    "LWTT" => new LWTTFlight(flightNumber, origin, destination, expectedTime, "Scheduled"),
                    _ => new NORMFlight(flightNumber, origin, destination, expectedTime, "Scheduled")
                };

                // Add the flight to the dictionary
                Flights[flightNumber] = newFlight;

                // Append the new Flight information to flights.csv 
                try
                {
                    using (StreamWriter writer = new StreamWriter("flights.csv", true))
                    {
                        writer.WriteLine($"{flightNumber},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{(specialRequestCode == "NONE" ? "" : specialRequestCode)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Could not write to flights.csv. Details: {ex.Message}");
                }

                // Display confirmation message
                Console.WriteLine($"\nFlight {flightNumber} has been added!");

                // Ask if the user wants to add another flight
                while (true)
                {
                    Console.Write("\nWould you like to add another flight? (Y/N): ");
                    string addAnother = Console.ReadLine().Trim().ToUpper();

                    if (addAnother == "Y")
                    {
                        addMoreFlights = true;
                        break;
                    }
                    else if (addAnother == "N")
                    {
                        addMoreFlights = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid input! Please enter Y or N.");
                    }
                }
            }
        }

        // Capitalizes the firts letter of origin and destination
        private string FormatLocation(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Find the opening bracket (if any)
            int bracketIndex = input.IndexOf("(");

            // If there's no bracket, just capitalize the first letter of the whole input
            if (bracketIndex == -1)
                return char.ToUpper(input[0]) + input.Substring(1).ToLower();

            // Separate the location name and the airport code
            string location = input.Substring(0, bracketIndex).Trim(); // City name
            string airportCode = input.Substring(bracketIndex).ToUpper().Trim(); // Keep (SIN) uppercase

            // Capitalize the first letter of the city and return the formatted result
            return char.ToUpper(location[0]) + location.Substring(1).ToLower() + " " + airportCode;
        }

        public override string ToString()
        {
            return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, " +
                $"Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
        }
    }
}
