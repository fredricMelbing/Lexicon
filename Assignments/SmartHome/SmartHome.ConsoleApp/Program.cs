using SmartHome.ConsoleApp.Entities;

namespace SmartHome.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			List<object> devices = new List<object>();

			devices.Add(new Washer("LG", "Laundry room", false, 7, 1.2));
			devices.Add(new Refrigerator("Samsung", "Kitchen", false, 8, 3.6));
			devices.Add(new Oven("Bosch", "Living room", false, 600, 2.5));
			devices.Add(new RobotVacuum("iRobot", "Living room", false, 500, 0.4));
			devices.Add(new CoffeeMachine("Nespresso", "Kitchen", false, 10, 0.3));

			RunMorningRoutine(devices); 
			
			Console.WriteLine();

			ReportAllEnergy(devices);

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
