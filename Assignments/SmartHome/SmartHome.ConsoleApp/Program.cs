using SmartHome.ConsoleApp.Entities;

namespace SmartHome.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			List<object> devices = new List<object>();

			devices.Add(new Washer("LG", 7, 1.2f));
			devices.Add(new Refrigerator("Samsung", 8, 3.6f));
			devices.Add(new Oven("Bosch", 600, 2.5f));
			devices.Add(new RobotVacuum("iRobot", 500, 0.4f));
			devices.Add(new CoffeeMachine("Nespresso", 10, 0.3f));

			RunMorningRoutine(devices); 
			
			Console.WriteLine(); 
			
			ReportAllEnergy(devices);

		}
		static void RunMorningRoutine(List<object> devices)
		{
			foreach (object device in devices)
			{
				if (device is Washer washer)
				{
					washer.StartWash();
					washer.StopWash();
				}
				else if (device is Refrigerator refrigerator)
				{
					refrigerator.StartCooling();
					refrigerator.StopCooling();
				}
				else if (device is Oven oven)
				{
					oven.StartHeating();
					oven.StopHeating();
				}
				else if (device is RobotVacuum robotVacuum)
				{
					robotVacuum.StartCleaning();
					robotVacuum.StopCleaning();
				}
				else if (device is CoffeeMachine coffeeMachine)
				{
					coffeeMachine.StartBrewing();
					coffeeMachine.StopBrewing();
				}
			}
		}
		static void ReportAllEnergy(List<object> devices)
		{
			foreach (object device in devices)
			{
				if (device is Washer washer)
				{
					washer.PrintWashEnergy();
				}
				else if (device is Refrigerator refrigerator)
				{
					refrigerator.PrintCoolingEnergy();
				}
				else if (device is Oven oven)
				{
					oven.PrintHeatingEnergy();
				}
				else if (device is RobotVacuum robotVacuum)
				{
					robotVacuum.PrintCleaningEnergy();
				}
				else if (device is CoffeeMachine coffeeMachine)
				{
					coffeeMachine.PrintBrewingEnergy();
				}
			}
		}
	}
}
