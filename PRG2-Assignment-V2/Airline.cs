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
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
            Flights = new Dictionary<string, Flight>();
        }
        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber)) // if flight num NOT in flights dict
            {
                Flights[flight.FlightNumber] = flight; // add flight to flights dict
                return true;
            }
            return false;
        }
        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }
        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees(); // for each flight, call method to get fee for that flight
            }
            return totalFees;
        }
        public override string ToString()
        {
            return $"{Code} - {Name} (Flights: {Flights.Count})";
        }
    }
}
