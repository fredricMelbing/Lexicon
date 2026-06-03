using SmartHome.ConsoleApp.Entities;

namespace SmartHome.ConsoleApp.Controllers
{
	internal class SmartHomeController
	{
		private List<Appliance> _devices = new List<Appliance>();
		public void AddDevice(Appliance device)
		{
			_devices.Add(device);
		}
		public void TurnOnAll()
		{
			_devices.ForEach(device => device.TurnOn());
		}
		public void TurnOffAll()
		{
			_devices.ForEach(device => device.TurnOff());
		}
		public void PrintStatusReport()
		{
			_devices.ForEach(device => Console.WriteLine(device.GetInfo()));
		}
		public double GetTotalDailyEnergyUsage()
		{
			return Math.Round(_devices.Sum(device => device.GetDailyEnergyUsage()), 2);
		}
	}
}
