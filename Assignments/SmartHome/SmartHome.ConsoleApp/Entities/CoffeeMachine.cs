namespace SmartHome.ConsoleApp.Entities
{
	internal class CoffeeMachine : Appliance
	{		
		public uint CupsPerBrew { get; }		
				
		public CoffeeMachine(string brand, string room, bool isOn, uint cupsPerBrew, double kWh) : base(brand, room, isOn, kWh)
		{
			CupsPerBrew = cupsPerBrew;
		}

		
		public void PrintBrewingEnergy()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per brew.");
		}

		public override string GetInfo()
		{
			return $"{Brand} in {Room}, CupsPerBrew: {CupsPerBrew}";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts brewing.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops brewing.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.25, 2);
		}
	}
}
