using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.ConsoleApp.Entities
{
	internal class Oven
	{
		public string Brand { get; }
		public uint MaxTemperature { get; }
		public float KWh { get; }

		public Oven(string brand, uint maxTemperature, float kWh)
		{
			Brand = brand;
			MaxTemperature = maxTemperature;
			KWh = kWh;
		}

		public void StartHeating()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts heating.");
		}
		public void StopHeating()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops heating.");
		}
		public void PrintHeatingEnergy()
		{			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per hour.");
		}
	}
}
