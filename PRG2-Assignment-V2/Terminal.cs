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

        public override string ToString()
        {
            return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, " +
                $"Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
        }
    }
}
