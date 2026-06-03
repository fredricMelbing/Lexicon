namespace SmartHome.ConsoleApp.Entities
{
	internal class Refrigerator : Appliance
	{
		public int Temperature { get; }

		public Refrigerator(string brand, string room, bool isOn, int temperature, double kWh) : base(brand, room, isOn, kWh)
		{
			Temperature = temperature;
		}


		public override string GetInfo()
		{
			return $"{base.GetInfo()} Temperature: {Temperature}°C";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} cooling.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} cooling.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 1, 2);			
		}
	}
}
