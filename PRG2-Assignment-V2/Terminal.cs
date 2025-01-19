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
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
            Airlines = new Dictionary<string, Airline>();
            Flights = new Dictionary<string, Flight>();
            BoardingGates = new Dictionary<string, BoardingGate>();
            GateFees = new Dictionary<string, double>();
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
        public override string ToString()
        {
            return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, " +
                $"Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
        }
    }
}
