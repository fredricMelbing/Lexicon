using SmartHome.ConsoleApp.Entities;

namespace SmartHome.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			List<Appliance> devices = new List<Appliance>();

			devices.Add(new Washer("LG", "Laundry room", false, 7, 1.2));
			devices.Add(new Refrigerator("Samsung", "Kitchen", false, 8, 3.6));
			devices.Add(new Oven("Bosch", "Living room", false, 600, 2.5));
			devices.Add(new RobotVacuum("iRobot", "Living room", false, 500, 0.4));
			devices.Add(new CoffeeMachine("Nespresso", "Kitchen", false, 10, 0.3));

			foreach (Appliance appliance in devices)
			{
				Console.WriteLine(appliance.GetInfo());
				appliance.TurnOn();
				Console.WriteLine($"{appliance.Brand} {appliance.GetType().Name.ToLower()} uses {appliance.GetDailyEnergyUsage()} kWh per cycle.");
				appliance.TurnOff();
			}


			//How it was done before:
			//List<object> devicesAsObjects = devices.Cast<object>().ToList();
			//RunMorningRoutine(devicesAsObjects);			
			//Console.WriteLine();
			//ReportAllEnergy(devicesAsObjects);
		}
		static void RunMorningRoutine(List<object> devices)
		{
			foreach (Appliance appliance in devices.OfType<Appliance>())
			{
				appliance.TurnOn();
				appliance.TurnOff();
			}
		}
		static void ReportAllEnergy(List<object> devices)
		{
			foreach (Appliance appliance in devices.OfType<Appliance>())
			{
				Console.WriteLine($"{appliance.Brand} {appliance.GetType().Name.ToLower()} uses {appliance.GetDailyEnergyUsage()} kWh per cycle.");
			}
		}
	}
}
