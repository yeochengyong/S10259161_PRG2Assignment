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
    public abstract class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public Flight(string flightNumber, string origin, string destination,
            DateTime expectedTime, string status = "Scheduled")
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }
        public int CompareTo(Flight other)
        {
            if (other == null)
                return 1; // Current flight comes first if other is null

            return this.ExpectedTime.CompareTo(other.ExpectedTime); // Sort flights in ascending order by time
        }

        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"{FlightNumber} | {Origin} -> {Destination} | {ExpectedTime} | {Status}";
        }
    }
}
