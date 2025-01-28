﻿using System;
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
    public class LWTTFlight : Flight
    {
        public static readonly double RequestFee = 500; // LWTT Code Request Fee

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "Scheduled")
            : base(flightNumber, origin, destination, expectedTime, status) { }

        public override double CalculateFees()
        {
            return 500 + RequestFee; // Base + LWTT
        }
    }
}
