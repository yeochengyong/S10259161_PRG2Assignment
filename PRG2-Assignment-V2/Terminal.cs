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
            GateFees = new Dictionary<string, double>
            {
                { "ArrivingFlight", 500 },
                { "DepartingFlight", 800 },
                { "BoardingGateBaseFee", 300 },
                { "DDJBCodeRequestFee", 300 },
                { "CFFTCodeRequestFee", 150 },
                { "LWTTCodeRequestFee", 500 }
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

        public void ListAllFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine($"List of Flights for {TerminalName}");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20}",
                              "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time");

            // Iterate through each airline
            foreach (var airline in Airlines.Values)
            {
                foreach (var flight in airline.Flights.Values) // Retrieve flights from airline
                {
                    Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20}",
                                      flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("hh:mm tt"));
                }
            }
        }

        public void ListAllBoardingGates()
        {
            Console.WriteLine("\n=========================================================");
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
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");

            Flight selectedFlight = null;

            // Prompt the user for the Flight Number
            while (selectedFlight == null)
            {
                Console.WriteLine("\nEnter Flight Number: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();

                // Extract airline code from flight number
                string airlineCode = flightNumber.Split(' ')[0];

                // Check if the airline exists and contains the flight
                if (Airlines.ContainsKey(airlineCode) && Airlines[airlineCode].Flights.ContainsKey(flightNumber))
                {
                    selectedFlight = Airlines[airlineCode].Flights[flightNumber]; // Retrieve flight from airline
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
                string gateName = Console.ReadLine().Trim().ToUpper();

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

            // Display Flight and Boarding Gate Information
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
            Console.Write("\nWould you like to update the status of the flight? (Y/N): ");
            string updateStatus = Console.ReadLine().Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.WriteLine("\n1. Delayed");
                Console.WriteLine("2. Boarding");
                Console.WriteLine("3. On Time");
                Console.Write("\nPlease select the new status of the flight: ");

                int statusChoice;
                while (!int.TryParse(Console.ReadLine().Trim(), out statusChoice) || statusChoice < 1 || statusChoice > 3)
                {
                    Console.Write("Invalid selection. Please select 1, 2, or 3: ");
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
                string airlineCode = ""; // Declare airlineCode once at the top

                while (true)
                {
                    Console.WriteLine("\nEnter Flight Number: ");
                    flightNumber = Console.ReadLine().Trim().ToUpper();

                    if (string.IsNullOrEmpty(flightNumber))
                    {
                        Console.WriteLine("Error: Flight Number cannot be empty. Please try again.");
                        continue;
                    }

                    // Extract airline code from flight number
                    airlineCode = flightNumber.Split(' ')[0]; // No redeclaration

                    // Ensure the airline exists
                    if (!Airlines.ContainsKey(airlineCode))
                    {
                        Console.WriteLine($"Error: Airline with code '{airlineCode}' not found. Please try again.");
                        continue;
                    }

                    // Check if the flight already exists inside the airline
                    if (Airlines[airlineCode].Flights.ContainsKey(flightNumber))
                    {
                        Console.WriteLine("Error: Flight Number already exists! Please enter a unique Flight Number.");
                        continue;
                    }

                    break;
                }

                string origin = "";
                while (true)
                {
                    Console.WriteLine("\nEnter Origin: ");
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
                    Console.WriteLine("\nEnter Destination: ");
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
                    Console.WriteLine("\nEnter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
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

                // Store the new flight inside the airline
                Flights[flightNumber] = newFlight;
                Airlines[airlineCode].Flights[flightNumber] = newFlight;

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

        public void DisplayFullFlightDetails()
        {
            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("==============================================");

            // list available airline
            Console.WriteLine("{0,-15} {1,-25}", "Airline Code", "Airline Name");
            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine("{0,-15} {1,-25}", airline.Code, airline.Name);
            }

            // 2 letter code
            Airline selectedAirline = null;
            while (selectedAirline == null)
            {
                Console.Write("\nEnter 2-Letter Airline Code (e.g., SQ, MH): ");
                string airlineCode = Console.ReadLine().Trim().ToUpper();

                if (Airlines.ContainsKey(airlineCode))
                {
                    selectedAirline = Airlines[airlineCode];
                }
                else
                {
                    Console.WriteLine("Error: Invalid airline code. Please try again.");
                }
            }

            Console.WriteLine("\n==========================================================");
            Console.WriteLine($"List of flights for {selectedAirline.Name} ({selectedAirline.Code}):");
            Console.WriteLine("==========================================================");
            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20}",
                              "Flight Number", "Airline Name", "Origin", "Destination");

            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20}",
                                  flight.FlightNumber, selectedAirline.Name, flight.Origin, flight.Destination);
            }

            // prompt user to select a flight number
            Flight selectedFlight = null;
            while (selectedFlight == null)
            {
                Console.Write("\nEnter Flight Number to view details: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();

                if (selectedAirline.Flights.ContainsKey(flightNumber))
                {
                    selectedFlight = selectedAirline.Flights[flightNumber];
                }
                else
                {
                    Console.WriteLine("Error: Invalid flight number. Please try again.");
                }
            }

            // display full flight details
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Full Flight Details:");
            Console.WriteLine("=============================================");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Destination: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Special Request Code: {(selectedFlight is NORMFlight ? "None" : selectedFlight.GetType().Name.Replace("Flight", ""))}");
            Console.WriteLine($"Boarding Gate: {(BoardingGates.Values.FirstOrDefault(g => g.AssignedFlight == selectedFlight)?.GateName ?? "None")}");
        }

        public void ModifyFlightDetails()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-15} {1,-25}", "Airline Code", "Airline Name");


            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine("{0,-15} {1,-25}", airline.Code, airline.Name);
            }

            // prompt user for airline code
            Airline selectedAirline = null;
            while (selectedAirline == null)
            {
                Console.Write("\nEnter 2-Letter Airline Code: ");
                string airlineCode = Console.ReadLine().Trim().ToUpper();

                if (Airlines.ContainsKey(airlineCode))
                {
                    selectedAirline = Airlines[airlineCode];
                }
                else
                {
                    Console.WriteLine("Error: Invalid airline code. Please try again.");
                }
            }

            // display flights for selected airline
            Console.WriteLine($"\nFlights for {selectedAirline.Name} ({selectedAirline.Code}):");

            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20}",
                              "Flight Number", "Origin", "Destination", "Expected Time");

            foreach (var flight in selectedAirline.Flights.Values)
            {
                Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20}",
                                  flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("dd/MM/yyyy HH:mm"));
            }

            // 
            Console.Write("\nChoose an option: \n[1] Modify a Flight\n[2] Delete a Flight\nSelect an option: ");
            string choice = Console.ReadLine().Trim();

            if (choice == "1")
            {
                ModifyExistingFlight(selectedAirline);
            }
            else if (choice == "2")
            {
                DeleteExistingFlight(selectedAirline);
            }
            else
            {
                Console.WriteLine("Error: Invalid option. Returning to menu.");
            }

            // display all updated flights after modify or deletion
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Updated Flight Details");
            Console.WriteLine("=============================================");
            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-15} {5,-10} {6,-10}",
                              "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time", "Status", "Boarding Gate");

            foreach (var airline in Airlines.Values)
            {
                foreach (var flight in airline.Flights.Values)
                {
                    // Find the assigned boarding gate for this flight
                    string boardingGateName = "None";
                    foreach (var gate in BoardingGates.Values)
                    {
                        if (gate.AssignedFlight == flight)
                        {
                            boardingGateName = gate.GateName;
                            break;
                        }
                    }

                    Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-15} {5,-10} {6,-10}",
                                      flight.FlightNumber, airline.Name, flight.Origin, flight.Destination, flight.ExpectedTime.ToString("dd/MM/yyyy HH:mm"),
                                      flight.Status, boardingGateName);
                }
            }
        }

        // MODIFY FLIGHT
        private void ModifyExistingFlight(Airline airline)
        {
            Flight selectedFlight = null;
            while (selectedFlight == null)
            {
                Console.Write("\nEnter Flight Number to modify: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();

                if (airline.Flights.ContainsKey(flightNumber))
                {
                    selectedFlight = airline.Flights[flightNumber];
                }
                else
                {
                    Console.WriteLine("Error: Invalid flight number. Please try again.");
                }
            }

            Console.WriteLine("\nWhat would you like to modify?");
            Console.WriteLine("[1] Origin");
            Console.WriteLine("[2] Destination");
            Console.WriteLine("[3] Expected Departure/Arrival Time");
            Console.WriteLine("[4] Status (On Time, Boarding, Delayed)");
            Console.WriteLine("[5] Special Request Code (CFFT, DDJB, LWTT, None)");
            Console.WriteLine("[6] Boarding Gate");

            Console.Write("\nSelect an option: ");
            string modifyChoice = Console.ReadLine().Trim();

            switch (modifyChoice)
            {
                case "1":
                    Console.Write("\nEnter new Origin: ");
                    selectedFlight.Origin = FormatLocation(Console.ReadLine().Trim());
                    break;
                case "2":
                    Console.Write("\nEnter new Destination: ");
                    selectedFlight.Destination = FormatLocation(Console.ReadLine().Trim());
                    break;
                case "3":
                    Console.Write("\nEnter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                    if (DateTime.TryParseExact(Console.ReadLine().Trim(), "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime newTime))
                    {
                        selectedFlight.ExpectedTime = newTime;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid time format. No changes made.");
                    }
                    break;
                case "4":
                    Console.Write("\nEnter new Status (On Time, Boarding, Delayed): ");
                    string newStatus = Console.ReadLine().Trim();
                    if (newStatus == "On Time" || newStatus == "Boarding" || newStatus == "Delayed")
                    {
                        selectedFlight.Status = newStatus;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid status. No changes made.");
                    }
                    break;
                case "5":
                    Console.Write("\nEnter new Special Request Code (CFFT, DDJB, LWTT, None): ");
                    string newCode = Console.ReadLine().Trim().ToUpper();
                    if (newCode == "CFFT" || newCode == "DDJB" || newCode == "LWTT" || newCode == "NONE")
                    {
                        selectedFlight = newCode switch
                        {
                            "CFFT" => new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status),
                            "DDJB" => new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status),
                            "LWTT" => new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status),
                            _ => new NORMFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status)
                        };
                        airline.Flights[selectedFlight.FlightNumber] = selectedFlight;
                        Console.WriteLine("\nSpecial Request Code updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid request code. No changes made.");
                    }
                    break;
                case "6":
                    Console.Write("\nEnter new Boarding Gate: ");
                    string gateName = Console.ReadLine().Trim().ToUpper();
                    if (BoardingGates.ContainsKey(gateName))
                    {
                        BoardingGate newGate = BoardingGates[gateName];

                        if (newGate.AssignedFlight == null)
                        {
                            // Unassign the flight from any previous gate
                            foreach (var gate in BoardingGates.Values)
                            {
                                if (gate.AssignedFlight == selectedFlight)
                                {
                                    gate.AssignedFlight = null;
                                    break;
                                }
                            }

                            // Assign to the new gate
                            newGate.AssignedFlight = selectedFlight;
                            Console.WriteLine($"\nFlight assigned to new Boarding Gate {gateName} successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Error: This Boarding Gate is already assigned to another flight. No changes made.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid Boarding Gate. No changes made.");
                    }
                    break;
                default:
                    Console.WriteLine("Error: Invalid option. No changes made.");
                    break;
            }

            Console.WriteLine("\nFlight details updated successfully!");
        }

        // DELETE FLIGHT
        private void DeleteExistingFlight(Airline airline)
        {
            Flight selectedFlight = null;
            while (selectedFlight == null)
            {
                Console.Write("\nEnter Flight Number to delete: ");
                string flightNumber = Console.ReadLine().Trim().ToUpper();

                if (airline.Flights.ContainsKey(flightNumber))
                {
                    selectedFlight = airline.Flights[flightNumber];
                }
                else
                {
                    Console.WriteLine("Error: Invalid flight number. Please try again.");
                }
            }

            Console.Write("\nAre you sure you want to delete this flight? (Y/N): ");
            string confirm = Console.ReadLine().Trim().ToUpper();
            if (confirm == "Y")
            {
                airline.Flights.Remove(selectedFlight.FlightNumber);
                Console.WriteLine("\nFlight successfully deleted!");
            }
            else
            {
                Console.WriteLine("\nDeletion cancelled.");
            }
        }
        public void DisplayScheduledFlights()
        {
            Console.WriteLine("\n===================================================================================================================");
            Console.WriteLine($"Flight Schedule for {TerminalName}");
            Console.WriteLine("=====================================================================================================================");

            // Collect all flights from all airlines
            List<Flight> allFlights = new List<Flight>();
            foreach (var airline in Airlines.Values)
            {
                allFlights.AddRange(airline.Flights.Values);
            }

            // Sort flights in chronological order
            allFlights.Sort();

            // Display table header (Fixed alignment)
            Console.WriteLine("{0,-12} {1,-22} {2,-25} {3,-25} {4,-30} {5,-12} {6,-15}",
                              "Flight No.", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival", "Status", "Boarding Gate");
            Console.WriteLine("======================================================================================================================");

            // Display flight details
            foreach (var flight in allFlights)
            {
                // Get the airline name
                string airlineCode = flight.FlightNumber.Split(' ')[0];
                string airlineName = Airlines.ContainsKey(airlineCode) ? Airlines[airlineCode].Name : "Unknown Airline";

                // Get the assigned boarding gate (if any)
                string boardingGateName = "Unassigned";
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.AssignedFlight == flight)
                    {
                        boardingGateName = gate.GateName;
                        break;
                    }
                }

                // Apply manual spacing to fix misalignment without changing class
                string formattedOrigin = flight.Origin.Length < 23 ? flight.Origin.PadRight(23) : flight.Origin;
                string formattedDestination = flight.Destination.Length < 23 ? flight.Destination.PadRight(23) : flight.Destination;

                // Format and display flight details
                Console.WriteLine("{0,-12} {1,-22} {2,-25} {3,-25} {4,-30} {5,-12} {6,-15}",
                                  flight.FlightNumber, airlineName, formattedOrigin, formattedDestination,
                                  flight.ExpectedTime.ToString("d/M/yyyy hh:mm:ss tt"), flight.Status, boardingGateName);
            }
        }

        // advanced feature (a)
        public void ProcessUnassignedFlights()
        {
            Console.WriteLine();
            Console.WriteLine("===============================================");
            Console.WriteLine("Processing Unassigned Flights to Boarding Gates");
            Console.WriteLine("===============================================");

            // Queue to store unassigned flights
            Queue<Flight> unassignedFlights = new Queue<Flight>();

            // Identify unassigned flights
            List<Flight> allFlights = Airlines.Values.SelectMany(a => a.Flights.Values).ToList();
            foreach (var flight in allFlights)
            {
                bool isAssigned = BoardingGates.Values.Any(g => g.AssignedFlight == flight);

                if (!isAssigned)
                {
                    unassignedFlights.Enqueue(flight);
                }
            }

            // Identify unassigned boarding gates
            List<BoardingGate> availableGates = BoardingGates.Values
                .Where(g => g.AssignedFlight == null)
                .ToList();

            Console.WriteLine($"\nTotal Unassigned Flights: {unassignedFlights.Count}");
            Console.WriteLine($"Total Unassigned Boarding Gates: {availableGates.Count}");
            Console.WriteLine();

            // Variables to track processing
            int flightsAssigned = 0;
            int gatesProcessed = 0;

            // Process flight assignments
            Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20} {5,-15}{6,-15}",
                                      "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time", "Status", "Boarding Gate");
            Console.WriteLine();

            while (unassignedFlights.Count > 0 && availableGates.Count > 0)
            {
                Flight flight = unassignedFlights.Dequeue();
                BoardingGate assignedGate = null;

                // Check for a gate that matches the special request code
                if (flight is CFFTFlight)
                    assignedGate = availableGates.FirstOrDefault(g => g.SupportsCFFT);
                else if (flight is DDJBFlight)
                    assignedGate = availableGates.FirstOrDefault(g => g.SupportsDDJB);
                else if (flight is LWTTFlight)
                    assignedGate = availableGates.FirstOrDefault(g => g.SupportsLWTT);
                else
                    assignedGate = availableGates.FirstOrDefault(g => !g.SupportsCFFT && !g.SupportsDDJB && !g.SupportsLWTT);


                if (assignedGate != null)
                {
                    assignedGate.AssignedFlight = flight;
                    flightsAssigned++;
                    gatesProcessed++;

                    // Remove assigned gate from available gates list
                    availableGates.Remove(assignedGate);

                    // Display the assigned flight details
                    string airlineName = Airlines[flight.FlightNumber.Split(' ')[0]].Name;
                    string boardingGateName = assignedGate.GateName;

                    Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-20} {4,-20} {5,-15}{6,-15}\n",
                                      flight.FlightNumber, airlineName, flight.Origin, flight.Destination,
                                      flight.ExpectedTime.ToString("d/M/yyyy hh:mm tt"), flight.Status, boardingGateName);
                }
            }

            // Summary of assignment process
            Console.WriteLine($"Total Flights Processed: {flightsAssigned}");
            Console.WriteLine($"Total Boarding Gates Processed: {gatesProcessed}");

            double assignedPercentage = (flightsAssigned > 0) ? ((double)flightsAssigned / Flights.Count) * 100 : 0;
            Console.WriteLine($"Flights Assigned Automatically: {flightsAssigned}");
        }

        // advanced feature (b)
        public void DisplayTotalFeePerAirline()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Total Fees Per Airline for Terminal 5");
            Console.WriteLine("=============================================");

            // Check if all flights have boarding gates assigned
            int unassignedFlights = Airlines.Values
                .SelectMany(a => a.Flights.Values)
                .Where(f => Flights.ContainsKey(f.FlightNumber))  // Only consider existing flights
                .Count(f => !BoardingGates.Values.Any(g => g.AssignedFlight == f));

            if (unassignedFlights > 0)
            {
                Console.WriteLine($"\nWarning: {unassignedFlights} flights do not have an assigned boarding gate.");
                Console.WriteLine("Please ensure all flights are assigned a gate before running this feature.");
                return;
            }

            double totalFees = 0;
            double totalDiscounts = 0;

            Console.WriteLine("\n{0,-20} {1,-15} {2,-15} {3,-15}", "Airline", "Subtotal", "Discounts", "Final Total");

            foreach (var airline in Airlines.Values)
            {
                double airlineTotalFee = 0;
                double airlineTotalDiscount = 0;
                int flightCount = 0;
                int flightsBefore11OrAfter9 = 0;
                int flightsWithSpecificOrigins = 0;
                int flightsWithNoSpecialRequest = 0;

                foreach (var flight in airline.Flights.Values)
                {
                    double flightFee = 0;
                    flightCount++;

                    // Apply Base Fees: Arriving / Departing Flight Fees
                    if (flight.Destination.Contains("Singapore (SIN)"))
                    {
                        flightFee += 500; // Arriving flight fee
                    }
                    if (flight.Origin.Contains("Singapore (SIN)"))
                    {
                        flightFee += 800; // Departing flight fee
                    }

                    // Apply Special Request Code Fees
                    if (flight is DDJBFlight)
                        flightFee += 300;
                    else if (flight is CFFTFlight)
                        flightFee += 150;
                    else if (flight is LWTTFlight)
                        flightFee += 500;

                    // Apply Boarding Gate Base Fee
                    flightFee += 300;

                    // Apply Promotional Discounts
                    if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                        flightsBefore11OrAfter9++;

                    if (flight.Origin.Contains("Dubai (DXB)") || flight.Origin.Contains("Bangkok (BKK)") || flight.Origin.Contains("Tokyo (NRT)"))
                        flightsWithSpecificOrigins++;

                    if (flight is NORMFlight) // Flight with no special request
                        flightsWithNoSpecialRequest++;

                    airlineTotalFee += flightFee;
                }

                // Apply Stackable Promotions
                airlineTotalDiscount += (flightCount / 3) * 350;  // Every 3 flights: $350 off
                airlineTotalDiscount += flightsBefore11OrAfter9 * 110; // Flights before 11AM or after 9PM: $110 off
                airlineTotalDiscount += flightsWithSpecificOrigins * 25; // Special origin discount: $25 off per flight
                airlineTotalDiscount += flightsWithNoSpecialRequest * 50; // Flights without special requests: $50 off

                // Additional 3% off for airlines with more than 5 flights
                if (flightCount > 5)
                    airlineTotalDiscount += airlineTotalFee * 0.03;

                double airlineFinalFee = airlineTotalFee - airlineTotalDiscount;

                // Print airline summary
                Console.WriteLine("\n{0,-20} ${1,-15:0.00} ${2,-15:0.00} ${3,-15:0.00}", airline.Name, airlineTotalFee, airlineTotalDiscount, airlineFinalFee);

                totalFees += airlineTotalFee;
                totalDiscounts += airlineTotalDiscount;
            }

            double finalTotal = totalFees - totalDiscounts;
            double discountPercentage = totalFees > 0 ? (totalDiscounts / totalFees) * 100 : 0;

            // Print final summary
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Terminal 5 Total Revenue Summary");
            Console.WriteLine("=============================================");
            Console.WriteLine("Total Fees Charged: ${0:0.00}", totalFees);
            Console.WriteLine("Total Discounts Given: ${0:0.00}", totalDiscounts);
            Console.WriteLine("Final Total Revenue Collected: ${0:0.00}", finalTotal);
            Console.WriteLine("Percentage of Discounts Over Total Fees: {0:0.00}%", discountPercentage);
            Console.WriteLine("=============================================");
        }

        public override string ToString()
        {
            return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, " +
                $"Flights: {Flights.Count}, Gates: {BoardingGates.Count}";
        }
    }
}
