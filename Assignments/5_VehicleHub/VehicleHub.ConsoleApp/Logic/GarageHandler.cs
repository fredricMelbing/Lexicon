using VehicleHub.ConsoleApp.Interfaces;
using VehicleHub.ConsoleApp.Models;
using VehicleHub.ConsoleApp.Storage;

namespace VehicleHub.ConsoleApp.Logic
{
	internal class GarageHandler : IGarageHandler
	{
		private Garage<Vehicle>? _garage;



		public void CreateGarage(uint capacity)
		{			
			_garage = new Garage<Vehicle>(capacity);
		}
		public bool ParkVehicle(Vehicle vehicle)
		{			
			if (_garage == null) throw new InvalidOperationException("Garage is not initialized.");
						
			bool isDuplicate = _garage.Any(v => v.RegistrationNumber.Equals(vehicle.RegistrationNumber, StringComparison.OrdinalIgnoreCase));
			if (isDuplicate)
			{
				return false;
			}

			return _garage.Park(vehicle);
		}
		public bool RemoveVehicle(string regNum)
		{
			if (_garage == null) return false;
			return _garage.Remove(regNum);
		}
		public IEnumerable<Vehicle> GetParkedVehicles()
		{
			if (_garage == null) return Enumerable.Empty<Vehicle>();
			return _garage;
		}
		public Vehicle? FindVehicleByRegNum(string regNum)
		{
			if (_garage == null) return null;
			return _garage.FirstOrDefault(v => v.RegistrationNumber.Equals(regNum, StringComparison.OrdinalIgnoreCase));
		}

		public Dictionary<string, int> GetVehicleTypeCount()
		{
			if (_garage == null) return new Dictionary<string, int>();

			return _garage
				.GroupBy(v => v.GetType().Name)
				.ToDictionary(g => g.Key, g => g.Count());
		}
		public IEnumerable<Vehicle> SearchVehiclesByProperties(string? vehicleType, string? color, uint? wheels)
		{
			if (_garage == null) return Enumerable.Empty<Vehicle>();

			IEnumerable<Vehicle> query = _garage;

			if (!string.IsNullOrWhiteSpace(vehicleType))
				query = query.Where(v => v.GetType().Name.Equals(vehicleType, StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(color))
				query = query.Where(v => v.Color.Equals(color, StringComparison.OrdinalIgnoreCase));

			if (wheels.HasValue)
				query = query.Where(v => v.NumberOfWheels == wheels.Value);

			return query.ToList();
		}
	}
}