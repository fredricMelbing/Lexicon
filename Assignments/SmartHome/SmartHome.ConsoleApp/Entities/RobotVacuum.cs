namespace SmartHome.ConsoleApp.Entities
{
	internal class RobotVacuum : Appliance
	{		
		public uint BatteryLevel { get; }

		public RobotVacuum(string brand, string room, bool isOn, uint batteryLevel, double kWh) : base(brand, room, isOn, kWh)
		{
			BatteryLevel = batteryLevel;
		}
		

		public override string GetInfo()
		{
			return $"{base.GetInfo()} BatteryLevel: {BatteryLevel}%";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} cleaning.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} cleaning.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.5, 2);
		}
	}
}
