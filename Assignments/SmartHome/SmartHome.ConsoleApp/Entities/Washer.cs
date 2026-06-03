using SmartHome.ConsoleApp.Interfaces;

namespace SmartHome.ConsoleApp.Entities
{
	internal class Washer: Appliance, ISchedulable
	{		
		public uint CapacityKg { get; }
		public DateTime NextRun { get; set; }

		public Washer(string brand, string room, bool isOn, uint capacityKg, double kWh) : base(brand, room, isOn, kWh)
		{			
			CapacityKg = capacityKg;
		}


		public void Schedule(DateTime time)
		{
			NextRun = time;
			Console.WriteLine($"{base.GetInfo()} scheduled for {time}.");
		}
		public override string GetInfo()
		{
			return $"{base.GetInfo()} Capacity: {CapacityKg} kg";			
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
			return Math.Round(KWh * 1.5, 2);
		}
	}	
}