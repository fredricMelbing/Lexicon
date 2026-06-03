namespace SmartHome.ConsoleApp.Entities
{
	internal class CoffeeMachine : Appliance
	{		
		public uint CupsPerBrew { get; }		
				
		public CoffeeMachine(string brand, string room, bool isOn, uint cupsPerBrew, double kWh) : base(brand, room, isOn, kWh)
		{
			CupsPerBrew = cupsPerBrew;
		}
				

		public override string GetInfo()
		{
			return $"{base.GetInfo()} CupsPerBrew: {CupsPerBrew}";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} brewing.");
		}
		public override void TurnOff()
		{			
			IsOn = false;			
			Console.WriteLine($"{base.GetInfo()} brewing.");			
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.25, 2);
		}
	}
}
