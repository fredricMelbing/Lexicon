namespace SmartHome.ConsoleApp.Entities
{
	internal class Washer: Appliance
	{		
		public uint CapacityKg { get; }
		

		public Washer(string brand, string room, bool isOn, uint capacityKg, double kWh) : base(brand, room, isOn, kWh)
		{			
			CapacityKg = capacityKg;			
		}

		public override string GetInfo()
		{
			return $"{Brand} in {Room}, Capacity: {CapacityKg} kg";			
		}
		public override void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts a washing program.");
		}
		public override void TurnOff()
		{
			IsOn = false;			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops a washing program.");
		}
		public override double GetDailyEnergyUsage()
		{			
			return Math.Round(KWh * 1.5, 2);
		}
	}	
}