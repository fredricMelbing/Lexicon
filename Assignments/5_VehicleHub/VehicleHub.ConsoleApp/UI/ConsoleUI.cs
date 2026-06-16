using System.Text.RegularExpressions;
using VehicleHub.ConsoleApp.Interfaces;
using VehicleHub.ConsoleApp.Models;

namespace VehicleHub.ConsoleApp.UI
{
	internal class ConsoleUI : IUI
	{
		private readonly IGarageHandler _handler;
		private Dictionary<string, Action> _mainMenuActions = new Dictionary<string, Action>();
		private Dictionary<string, string> _parkingTypes = new Dictionary<string, string>();

		public ConsoleUI(IGarageHandler handler)
		{
			_handler = handler;
			InitializeMenus();
		}

		private void GenerateData(uint capacity)
		{
			List<Vehicle> vehicles = new List<Vehicle>
			{
				new Car("ABC123", "Röd", 4, "Bensin"),
				new Airplane("XYZ789", "Blå", 6, 2),
				new Boat("BOAT1", "Vit", 0, 50),
				new Car("DEF456", "Grön", 4, "El"),
				new Bus("BUS123", "Röd", 6, 50),
				new Motorcycle("MOTO1", "Svart", 2, 600)
			};

			foreach (var vehicle in vehicles)
			{
				if (!_handler.ParkVehicle(vehicle))
					break;
			}
		}

		private void InitializeMenus()
		{
			_mainMenuActions = new Dictionary<string, Action>
			{
				{ "1", ListAllVehicles },
				{ "2", SumAllVehiclesByType },
				{ "3", ShowParkingMenu },
				{ "4", UnparkVehicle },
				{ "5", SearchVehicle },
				{ "0", () => Environment.Exit(0) }
			};

			var subClass = typeof(Vehicle).Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(Vehicle)));

			foreach (Type type in subClass)
				_parkingTypes.Add((_parkingTypes.Count + 1).ToString(), type.Name);
		}

		public void Run()
		{
			Console.WriteLine("=== WELCOME TO VEHICLEHUB MANAGER 1.0 ===");
			InitializeGarage();
			MainMenuLoop();
		}
		private void InitializeGarage()
		{			
			while (true)
			{
				Console.Write("Enter the number of parking spaces the garage should have: ");
				if (uint.TryParse(Console.ReadLine(), out uint capacity) && capacity > 0)
				{
					_handler.CreateGarage(capacity);
					Console.WriteLine($"A garage with {capacity} spaces has been created!");
					GenerateData(capacity);
					break;
				}
				Console.WriteLine("Invalid input. Please enter a positive integer.");
			}
		}
		private void MainMenuLoop()
		{
			while (true)
			{
				Console.WriteLine("------ MAIN MENU ------");
				foreach (var option in _mainMenuActions)
				{
					if (option.Key == "0")
						Console.WriteLine($"{option.Key}. EXIT");
					else
						Console.WriteLine($"{option.Key}. {Regex.Replace(option.Value.Method.Name, @"(?<=[a-z])([A-Z])", " $1")}");
				}
				Console.Write("Select an option: ");

				string choice = Console.ReadLine() ?? string.Empty;
				Console.WriteLine();

				if (_mainMenuActions.ContainsKey(choice))
				{
					Console.Clear();
					_mainMenuActions[choice].Invoke();
				}
				else
				{
					Console.Clear();
					Console.WriteLine("Invalid option. Please try again.");
				}
			}
		}
		private void ShowParkingMenu()
		{
			//TODO: Refactor Create classes to create a new instance of the class based on the type of vehicle the user wants to park with Dictionary.
			//TODO EXTRA: Refactor more dynamic and use reflection to create instances of the selected vehicle type.

			Console.WriteLine("------ PARKING MENU ------");
			Console.WriteLine("--- CHOOSE VEHICLE TYPE TO PARK ---");

			foreach (var option in _parkingTypes)
				Console.WriteLine($"{option.Key}. {option.Value}");

			string choice = Console.ReadLine() ?? string.Empty;
			if (!_parkingTypes.ContainsKey(choice))
			{
				Console.WriteLine("Invalid vehicle type. Returning to main menu.");
				return;
			}

			Console.Write("Enter registration number: ");
			string regNum = Console.ReadLine() ?? string.Empty;

			if (_handler.FindVehicleByRegNum(regNum) != null)
			{
				Console.WriteLine($"Error: A vehicle with registration number {regNum.ToUpper()} is already parked here!");
				return;
			}

			Console.Write("Enter color: ");
			string color = Console.ReadLine() ?? string.Empty;

			Console.Write("Enter number of wheels: ");
			uint.TryParse(Console.ReadLine(), out uint wheels);

			if (_parkingTypes[choice] == "Car")
			{
				Console.Write("Enter fuel type (e.g., Diesel, Gasoline): ");
				string fuelType = Console.ReadLine() ?? string.Empty;
				PrintParkingResult(_handler.ParkVehicle(new Car(regNum, color, wheels, fuelType)));
			}
			else if (_parkingTypes[choice] == "Airplane")
			{
				Console.Write("Enter number of engines: ");
				uint.TryParse(Console.ReadLine(), out uint engines);
				PrintParkingResult(_handler.ParkVehicle(new Airplane(regNum, color, wheels, engines)));
			}
			else if (_parkingTypes[choice] == "Boat")
			{
				Console.Write("Enter boat length in feet: ");
				uint.TryParse(Console.ReadLine(), out uint length);
				PrintParkingResult(_handler.ParkVehicle(new Boat(regNum, color, wheels, length)));
			}
			else if (_parkingTypes[choice] == "Bus")
			{
				Console.Write("Enter seating capacity: ");
				uint.TryParse(Console.ReadLine(), out uint capacity);
				PrintParkingResult(_handler.ParkVehicle(new Bus(regNum, color, wheels, capacity)));
			}
			else if (_parkingTypes[choice] == "Motorcycle")
			{
				Console.Write("Enter engine displacement in cc: ");
				uint.TryParse(Console.ReadLine(), out uint displacement);
				PrintParkingResult(_handler.ParkVehicle(new Motorcycle(regNum, color, wheels, displacement)));
			}
		}
		private void ListAllVehicles()
		{
			var vehicles = _handler.GetParkedVehicles();
			Console.WriteLine("--- PARKED VEHICLES ---");

			int count = 0;
			foreach (var vehicle in vehicles)
			{
				Console.WriteLine(vehicle.GetInfo());
				count++;
			}

			if (count == 0) Console.WriteLine("Garage is empty.");
			Console.WriteLine();
		}
		private void SumAllVehiclesByType()
		{
			var counts = _handler.GetVehicleTypeCount();
			Console.WriteLine("--- VEHICLE TYPES IN THE GARAGE ---");

			if (!counts.Any())
			{
				Console.WriteLine("No vehicles in the garage.");
			}
			else
			{
				foreach (var kvp in counts)
				{
					Console.WriteLine($"{kvp.Key}: {kvp.Value} st");
				}
			}
			Console.WriteLine();
		}

		private void UnparkVehicle()
		{
			Console.Write("Enter registration number of the vehicle to be removed: ");
			string regNum = Console.ReadLine() ?? string.Empty;

			if (_handler.RemoveVehicle(regNum))
			{
				Console.WriteLine($"Vehicle with registration number {regNum.ToUpper()} has been removed!");
			}
			else
			{
				Console.WriteLine($"No vehicle found with registration number {regNum.ToUpper()} in the garage.");
			}
		}
		private void SearchVehicle()
		{
			Console.WriteLine("--- SEARCH VEHICLES BY PROPERTIES (Leave blank to ignore a filter) ---");

			Console.Write("Do you want to search by Type?: y/n ");
			string typeInput = string.Empty;

			if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
			{
				Console.WriteLine();
				foreach (var option in _parkingTypes)
					Console.WriteLine($"{option.Key}. {option.Value}");

				string choice = Console.ReadKey().KeyChar.ToString() ?? string.Empty;
				Console.WriteLine();
				
				if (_parkingTypes.ContainsKey(choice))
				{
					typeInput = _parkingTypes[choice];
					Console.Clear();
					Console.WriteLine($"Search by type: {typeInput}");
				}
				else
				{
					Console.Clear();
					Console.WriteLine("Invalid choice. Searching with other criteria.");
				}
			}
			Console.Write("Search by color: ");
			string color = Console.ReadLine() ?? string.Empty;

			Console.Write("Search by number of wheels: ");
			string wheelsInput = Console.ReadLine() ?? string.Empty;
			uint? wheels = null;

			if (uint.TryParse(wheelsInput, out uint wheelsValue))
			{
				wheels = wheelsValue;
			}

			var results = _handler.SearchVehiclesByProperties(typeInput, color, wheels);

			Console.WriteLine("--- SEARCH RESULTS ---");
			int count = 0;
			foreach (var vehicle in results)
			{
				Console.WriteLine(vehicle.GetInfo());
				count++;
			}

			if (count == 0) Console.WriteLine("No vehicles matched your search.");
			Console.WriteLine();
		}
		private void PrintParkingResult(bool success)
		{
			if (success)
				Console.WriteLine("Vehicle has been parked successfully!");
			else
				Console.WriteLine("Failed to park vehicle. Garage is full!");
		}
	}
}