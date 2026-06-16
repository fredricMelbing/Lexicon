using VehicleHub.ConsoleApp.Interfaces;
using VehicleHub.ConsoleApp.Logic;
using VehicleHub.ConsoleApp.UI;

namespace VehicleHub.ConsoleApp
{
	internal class Manager
	{
		public void StartApplication()
		{			
			IGarageHandler handler = new GarageHandler();						
			
			IUI ui = new ConsoleUI(handler);

			ui.Run();
		}
	}
}
