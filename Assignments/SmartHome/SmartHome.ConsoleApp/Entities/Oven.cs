namespace SmartHome.ConsoleApp.Entities
{
	internal class Oven : Appliance
	{		
		public uint MaxTemperature { get; }

		public Oven(string brand, string room, bool isOn, uint maxTemperature, double kWh) : base(brand, room, isOn, kWh)
		{
			MaxTemperature = maxTemperature;
		}


		public override string GetInfo()
		{
			return $"{Brand} in {Room}, MaxTemperature: {MaxTemperature}°C";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts heating.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops heating.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.2, 2);
		}
	}
}
