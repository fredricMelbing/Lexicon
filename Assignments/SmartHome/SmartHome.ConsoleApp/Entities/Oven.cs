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
			return $"{base.GetInfo()} MaxTemperature: {MaxTemperature}°C";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} heating.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} heating.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.2, 2);
		}
	}
}
