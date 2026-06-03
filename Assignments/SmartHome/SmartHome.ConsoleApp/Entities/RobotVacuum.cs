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
			return $"{Brand} in {Room}, BatteryLevel: {BatteryLevel}%";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts cleaning.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops cleaning.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.5, 2);
		}
	}
}
