namespace VehicleHub.ConsoleApp.Interfaces
{
	internal interface IVehicle
	{
		string RegistrationNumber { get; }
		string Color { get; }
		uint NumberOfWheels { get; }
	}
}
