using SmartHome.ConsoleApp.Interfaces;

namespace SmartHome.ConsoleApp.Entities
{
	internal class AirConditioner : Appliance, ISchedulable
	{
		public double TargetTemperature { get; }
		public DateTime NextRun { get; set; }

		public AirConditioner(string brand, string room, bool isOn, double targetTemperature, double kWh) : base(brand, room, isOn, kWh)
		{
			TargetTemperature = targetTemperature;
		}


		public void Schedule(DateTime time)
		{
			NextRun = time;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} scheduled for {time}.");
		}
		public override string GetInfo()
		{
			return $"{base.GetInfo()} SetTemperature: {TargetTemperature}°C";
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{base.GetInfo()} In Process of changing Temperature.");
		}
		public override void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{base.GetInfo()} DONE changed Temperature.");
		}
		public override double GetDailyEnergyUsage()
		{
			return Math.Round(KWh * 0.75, 2);
		}
	}
}
