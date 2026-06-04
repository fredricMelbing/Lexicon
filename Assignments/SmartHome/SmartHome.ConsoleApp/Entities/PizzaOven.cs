namespace SmartHome.ConsoleApp.Entities
{
	internal class PizzaOven : Oven
	{
		public PizzaOven(string brand, string room, bool isOn, uint maxTemperature, double kWh) : base(brand, room, isOn, maxTemperature, kWh)
		{
		}


		public override void TurnOn()
		{
			Console.WriteLine("Pizza oven starts at extra high temperature.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.5, 2);
		}
	}
}
