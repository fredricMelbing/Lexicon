using SmartHome.ConsoleApp.Controllers;
using SmartHome.ConsoleApp.Entities;
using SmartHome.ConsoleApp.Interfaces;

namespace SmartHome.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			SmartHomeController controller = new SmartHomeController();
			
			controller.AddDevice(new Washer("LG", "Laundry room", false, 7, 1.2));
			controller.AddDevice(new Refrigerator("Samsung", "Kitchen", false, 8, 3.6));
			controller.AddDevice(new Oven("Bosch", "Living room", false, 600, 2.5));
			controller.AddDevice(new RobotVacuum("iRobot", "Living room", false, 100, 0.4));
			controller.AddDevice(new CoffeeMachine("Nespresso", "Kitchen", false, 10, 0.3));
			controller.AddDevice(new Dishwasher("Siemens", "Kitchen", false, 12, 1.8));
			controller.AddDevice(new SmartLamp("Philips Hue", "Living room", false, 0.05, 0.1));
			controller.AddDevice(new PizzaOven("Gourmet", "Kitchen", false, 750, 2.8));
			controller.AddDevice(new AirConditioner("Mitsubishi", "Vardagsrum", false, 21.5, 1.2));
			

			SmartLamp lamp1 = new SmartLamp("IKEA", "Hallway", false, 80, 0.1);
			Appliance lamp2 = lamp1;
			lamp1.TurnOn();
			lamp2.TurnOn();



			controller.PrintStatusReport();
			Console.WriteLine();

			controller.TurnOnAll();
			Console.WriteLine();

			double totalEnergy = controller.GetTotalDailyEnergyUsage();
			Console.WriteLine($"Total daily energy usage: {totalEnergy} kWh");			
			Console.WriteLine();
			
			controller.TurnOffAll();
			Console.WriteLine();

			controller.ScheduleAllSchedulableDevices(DateTime.Now.AddHours(2));
			Console.WriteLine();

			List<ISchedulable> schedulableDevices = controller.GetSchedulableDevices();
			schedulableDevices.ForEach(device => Console.WriteLine($"{device.GetType().Name}: {device.NextRun}"));
			Console.WriteLine();			
			
			Appliance? foundDevice = controller.FindDeviceByBrand("LG");
			if (foundDevice != null)
			{
				if (foundDevice is ISchedulable schedulableDevice)
					schedulableDevice.Schedule(DateTime.Now.AddHours(1));
				else
					foundDevice.TurnOn();
			}
		}
	}
}