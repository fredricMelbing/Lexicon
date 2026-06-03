namespace SmartHome.ConsoleApp.Entities
{
	internal class CoffeeMachine
	{
		public string Brand { get; }
		public uint CupsPerBrew { get; }
		public float KWh { get; }

		public CoffeeMachine(string brand, uint cupsPerBrew, float kWh)
		{
			Brand = brand;
			CupsPerBrew = cupsPerBrew;
			KWh = kWh;
		}

		public void StartBrewing()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts brewing.");
		}
		public void StopBrewing()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops brewing.");
		}
		public void PrintBrewingEnergy()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per brew.");
		}
	}
}
