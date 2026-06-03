namespace SmartHome.ConsoleApp.Entities
{
	internal class Dishwasher : Appliance
	{
		public uint Kuvert { get; }
		

		public Dishwasher(string brand, string room, bool isOn, uint kuvert, double kWh) : base(brand, room, isOn, kWh)
		{
			Kuvert = kuvert;
		}

		public override string GetInfo()
		{
			return $"{base.GetInfo()} Kuvert: {Kuvert}";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} In Process of Washing.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} DONE Washing.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 2.5, 2);
		}
	}
}
