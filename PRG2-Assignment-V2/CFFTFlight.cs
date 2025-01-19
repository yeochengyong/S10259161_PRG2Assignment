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
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; private set; }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled", double requestFee = 150)
            : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = requestFee;
        }
        public override double CalculateFees()
        {
            return 500 + RequestFee;
        }
    }
}
