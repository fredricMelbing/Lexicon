using SmartHome.ConsoleApp.Interfaces;

namespace SmartHome.ConsoleApp.Entities
{
	internal class RobotVacuum : Appliance, ISchedulable
	{		
		public uint BatteryLevel { get; }
		public DateTime NextRun { get; set; }

		public RobotVacuum(string brand, string room, bool isOn, uint batteryLevel, double kWh) : base(brand, room, isOn, kWh)
		{
			BatteryLevel = batteryLevel;
		}
		

		public void Schedule(DateTime time)
		{
			NextRun = time;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} scheduled for {time}.");
		}
		public override string GetInfo()
		{
			return $"{base.GetInfo()} BatteryLevel: {BatteryLevel}%";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} In Process of Cleaning.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} DONE Cleaning.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.5, 2);
		}
	}
}
