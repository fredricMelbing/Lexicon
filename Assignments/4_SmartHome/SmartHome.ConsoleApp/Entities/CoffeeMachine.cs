using SmartHome.ConsoleApp.Interfaces;

namespace SmartHome.ConsoleApp.Entities
{
	internal class CoffeeMachine : Appliance, ISchedulable
	{		
		public uint CupsPerBrew { get; }
		public DateTime NextRun { get; set; }

		public CoffeeMachine(string brand, string room, bool isOn, uint cupsPerBrew, double kWh) : base(brand, room, isOn, kWh)
		{
			CupsPerBrew = cupsPerBrew;
		}


		public void Schedule(DateTime time)
		{
			NextRun = time;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} scheduled for {time}.");
		}
		public override string GetInfo()
		{
			return $"{base.GetInfo()} CupsPerBrew: {CupsPerBrew}";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} In Process of Brewing.");
		}
		public override void TurnOff()
		{			
			IsOn = false;			
			Console.WriteLine($"{base.GetInfo()} DONE Brewing.");			
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.25, 2);
		}
	}
}
