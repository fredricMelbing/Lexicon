namespace SmartHome.ConsoleApp.Entities
{
	internal abstract class Appliance
	{
		public string Brand { get; }
		public string Room { get; }
		public bool IsOn { get; protected set; }
		public double KWh { get; }

		public Appliance(string brand, string room, bool isOn, double kWh)
		{
			Brand = brand;
			Room = room;
			IsOn = isOn;
			KWh = kWh;
		}
		public virtual string GetInfo()
		{
			return $"{Brand} {this.GetType().Name.ToLower()} in {Room} is {(IsOn ? "turned on" : "turned off")}.";
		}
		public virtual void TurnOn()
		{
			IsOn = true;			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts");
		}
		public virtual void TurnOff()
		{
			IsOn = false;			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops");
		}
		public virtual double GetDailyEnergyUsage()
		{
			return 0;
		}
	}
}
