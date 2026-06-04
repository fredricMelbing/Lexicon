using SmartHome.ConsoleApp.Entities;
using SmartHome.ConsoleApp.Interfaces;

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
		public void ScheduleAllSchedulableDevices(DateTime time)
		{
			if (_devices.OfType<ISchedulable>().Any())
				_devices.OfType<ISchedulable>().ToList().ForEach(device => device.Schedule(time));
			else
				Console.WriteLine("No schedulable devices found.");
		}
		internal List<ISchedulable> GetSchedulableDevices()
		{
			List<ISchedulable> result = new List<ISchedulable>();
			if (_devices.OfType<ISchedulable>().Any())
			{
				_devices.OfType<ISchedulable>().ToList().ForEach(result.Add);
			}
			else
				Console.WriteLine("No schedulable devices found.");
			return result;
		}
		public Appliance? FindDeviceByBrand(string brand)
		{
			var device = _devices.FirstOrDefault(device => device.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
			if (device == null)
			{
				Console.WriteLine($"No device found with brand: {brand}");
				return null;
			}
			return device;
		}
	}
}