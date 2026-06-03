namespace SmartHome.ConsoleApp.Entities
{
	internal class Appliance
	{
		public string Brand { get; }
		public string Room { get; }
		public bool IsOn { get; protected set; }

		public Appliance(string brand, string room, bool isOn)
		{
			Brand = brand;
			Room = room;
			IsOn = isOn;
		}
		public virtual string GetInfo()
		{
			return $"{Brand} in {Room}";
		}
		public virtual void TurnOn()
		{
			IsOn = true;
			Console.WriteLine($"{GetInfo()} is now on.");
		}
		public virtual void TurnOff()
		{
			IsOn = false;
			Console.WriteLine($"{GetInfo()} is now off.");
		}
		public virtual double GetDailyEnergyUsage()
		{
			return 0;
		}
	}
}
