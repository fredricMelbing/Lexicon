using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.ConsoleApp.Entities
{
	internal class RobotVacuum
	{
		public string Brand { get; }
		public uint BatteryLevel { get; }
		public float KWh { get; }
		public RobotVacuum(string brand, uint batteryLevel, float kWh)
		{
			Brand = brand;
			BatteryLevel = batteryLevel;
			KWh = kWh;
		}

		public void StartCleaning()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts cleaning.");
		}
		public void StopCleaning()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops cleaning.");
		}
		public void PrintCleaningEnergy()
		{			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per cleaning.");
		}
	}
}
