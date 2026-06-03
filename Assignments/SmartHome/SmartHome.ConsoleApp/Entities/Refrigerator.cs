using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.ConsoleApp.Entities
{
	internal class Refrigerator
	{
		public string Brand { get; }
		public int Temperature { get; }
		public float KWh { get; }

		public Refrigerator(string brand, int temperature, float kWh)
		{
			Brand = brand;
			Temperature = temperature;
			KWh = kWh;
		}

		public void StartCooling()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts cooling.");
		}
		public void StopCooling()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops cooling.");
		}
		public void PrintCoolingEnergy()
		{			
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per day.");
		}
	}
}
