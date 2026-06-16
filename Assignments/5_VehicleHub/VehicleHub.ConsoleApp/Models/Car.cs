namespace VehicleHub.ConsoleApp.Models
{
	internal class Car : Vehicle
	{
		public string FuelType { get; set; }

		public Car(string regNum, string color, uint wheels, string fuelType)
			: base(regNum, color, wheels)
		{
			FuelType = fuelType; //TODO Extra: Car property (Gasoline/Diesel/Electric)
		}

		public override string GetInfo()
		{
			return $"{base.GetInfo()}, Fuel Type: {FuelType}";
		}
	}
}
