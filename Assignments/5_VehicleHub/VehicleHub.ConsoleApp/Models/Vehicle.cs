using VehicleHub.ConsoleApp.Interfaces;

namespace VehicleHub.ConsoleApp.Models
{
	internal abstract class Vehicle : IVehicle
	{
		public string RegistrationNumber { get; }
		public string Color { get; }
		public uint NumberOfWheels { get; }

		protected Vehicle(string regNum, string color, uint wheels)
		{
			RegistrationNumber = regNum.ToUpper();
			Color = color;
			NumberOfWheels = wheels;
		}
		public virtual string GetInfo()
		{			
			return $"[{this.GetType().Name}] Regnr: {RegistrationNumber}, Color: {Color}, Wheels: {NumberOfWheels}";
		}
	}
}