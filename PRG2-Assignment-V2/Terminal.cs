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

            // Step 1: Prompt the user for the Flight Number
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

            // Step 2: Prompt for a Boarding Gate
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

            // Step 3: Display Flight and Boarding Gate Information in the Correct Format
            Console.WriteLine("\nFlight Number: " + selectedFlight.FlightNumber);
            Console.WriteLine("Origin: " + selectedFlight.Origin);
            Console.WriteLine("Destination: " + selectedFlight.Destination);
            Console.WriteLine("Expected Time: " + selectedFlight.ExpectedTime.ToString("d/M/yyyy hh:mm:ss tt"));
            Console.WriteLine("Special Request Code: " + (selectedFlight is NORMFlight ? "None" : selectedFlight.GetType().Name.Replace("Flight", "")));

            Console.WriteLine("\nBoarding Gate Name: " + selectedGate.GateName);
            Console.WriteLine("Supports DDJB: " + (selectedGate.SupportsDDJB ? "True" : "False"));
            Console.WriteLine("Supports CFFT: " + (selectedGate.SupportsCFFT ? "True" : "False"));
            Console.WriteLine("Supports LWTT: " + (selectedGate.SupportsLWTT ? "True" : "False"));

            // Step 4: Prompt user to update Flight Status
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

            // Step 5: Assign the Flight to the Boarding Gate
            selectedGate.AssignedFlight = selectedFlight;
            Console.WriteLine($"\nBoarding Gate {selectedGate.GateName} successfully assigned to Flight {selectedFlight.FlightNumber}.");
        }
        public override string ToString()
        {
            return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, " +
                $"Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
        }
    }
}
