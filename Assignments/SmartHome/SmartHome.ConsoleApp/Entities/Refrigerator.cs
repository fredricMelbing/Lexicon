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
			return $"{Brand} in {Room}, Temperature: {Temperature}°C";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts cooling.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops cooling.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 1, 2);			
		}
	}
}
