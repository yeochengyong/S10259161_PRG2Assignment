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
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight AssignedFlight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        public double CalculateFees()
        {
            return AssignedFlight != null ? 300 : 0; // if flight assigned to this gate, return fee of 300. otherwise return 0.
        }

        public override string ToString()
        {
            return $"{GateName} | Supports CFFT: {SupportsCFFT}, DDJB: {SupportsDDJB}, LWTT: {SupportsLWTT}";
        }
    }
}
