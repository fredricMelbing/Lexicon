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

		private void GenerateData(int capacity)
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
				{ "3", ParkVehicle },
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
				if (int.TryParse(Console.ReadLine(), out int capacity) && capacity > 0 && capacity <= int.MaxValue)
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
		private void ParkVehicle()
		{
			Console.WriteLine("------ PARKING MENU ------");
			int freeSpaces = _handler.GetAvailableSpaces();
			if (freeSpaces <= 0)
			{				
				Console.WriteLine("Sorry, Garage are full! Going back to Main Menu");				
				return;
			}
			Console.WriteLine($"--- CHOOSE VEHICLE TYPE TO PARK: {freeSpaces} vacant spaces left ---");

			foreach (var option in _parkingTypes)
				Console.WriteLine($"{option.Key}. {option.Value}");

			string choice = Console.ReadLine() ?? string.Empty;
			if (!_parkingTypes.ContainsKey(choice))
			{
				Console.WriteLine("Invalid vehicle type. Returning to main menu.");
				return;
			}

			Type? vehicleType = typeof(Vehicle).Assembly.GetTypes()
				.FirstOrDefault(t => t.Name == _parkingTypes[choice]);

			if (vehicleType == null)
			{				
				Console.WriteLine("Could not find the selected vehicle type. Returning to main menu.");				
				return;
			}
			CreateVehicleDynamically(vehicleType);
		}
		private void CreateVehicleDynamically(Type type)
		{			
			Console.Write("Enter registration number: ");
			string regNum = (Console.ReadLine() ?? string.Empty).ToUpper();
			if (regNum == string.Empty || regNum == "")
			{
				Console.WriteLine($"Not valid: Registration number {regNum}!");
				return;				
			}

			if (_handler.FindVehicleByRegNum(regNum) != null)
			{
				Console.WriteLine($"Not valid: A vehicle with registration number {regNum} is already parked here!");
				return;
			}

			Console.Write("Enter color: ");
			string color = Console.ReadLine() ?? "";

			Console.Write("Enter number of wheels: ");
			uint.TryParse(Console.ReadLine(), out uint wheels);

			var uniqueProperties = type.GetProperties()
				.Where(p => p.DeclaringType == type)
				.ToList();
						
			var constructorArgs = new List<object> { regNum, color, wheels };
						
			foreach (var prop in uniqueProperties)
			{
				Console.Write($"Enter {prop.Name}: ");
				string input = Console.ReadLine() ?? "";
								
				try
				{
					object convertedValue = Convert.ChangeType(input, prop.PropertyType);
					constructorArgs.Add(convertedValue);
				}
				catch
				{					
					Console.WriteLine($"Invalid datatype: {prop.Name}. Cancel Process ");					
					return;
				}
				Console.Clear();
			}

			try
			{
				object? newVehicle = Activator.CreateInstance(type, constructorArgs.ToArray());

				if (newVehicle is Vehicle vehicle)
				{
					PrintParkingResult(_handler.ParkVehicle(vehicle));
				}
			}
			catch
			{				
				Console.WriteLine("Something went wrong while creating the vehicle. Please check your inputs and try again.");				
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
				Console.WriteLine("No vehicles in the garage.");
			else
			{
				foreach (var kvp in counts)
					Console.WriteLine($"{kvp.Key}: {kvp.Value} st");
			}
			Console.WriteLine();
		}

		private void UnparkVehicle()
		{
			Console.Write("Enter registration number of the vehicle to be removed: ");
			string regNum = Console.ReadLine() ?? string.Empty;

			if (_handler.RemoveVehicle(regNum))
				Console.WriteLine($"Vehicle with registration number {regNum.ToUpper()} has been removed!");
			else
				Console.WriteLine($"No vehicle found with registration number {regNum.ToUpper()} in the garage.");
		}
		private void SearchVehicle()
		{
			Console.WriteLine("--- SEARCH VEHICLES BY PROPERTIES (Leave blank to ignore a filter) ---");

			Console.Write("Do you want to search by Type?: y/n ");
			string typeInput = string.Empty;			
			string typechoice = Console.ReadKey().KeyChar.ToString() ?? string.Empty;
						
			if (typechoice.ToLower().Equals("y"))
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
			else
			{				
				Console.Clear();
				Console.WriteLine("Searching with other criteria.");
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