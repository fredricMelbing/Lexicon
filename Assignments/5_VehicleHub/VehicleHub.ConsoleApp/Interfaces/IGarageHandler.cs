using VehicleHub.ConsoleApp.Models;

namespace VehicleHub.ConsoleApp.Interfaces
{
	internal interface IGarageHandler
	{
		void CreateGarage(uint capacity);
		bool ParkVehicle(Vehicle vehicle);
		bool RemoveVehicle(string regNum);
		IEnumerable<Vehicle> GetParkedVehicles();
		Vehicle? FindVehicleByRegNum(string regNum);
		Dictionary<string, int> GetVehicleTypeCount();
		IEnumerable<Vehicle> SearchVehiclesByProperties(string? vehicleType, string? color, uint? wheels);
	}
}
