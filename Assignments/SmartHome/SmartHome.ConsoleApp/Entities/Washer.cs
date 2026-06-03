using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.ConsoleApp.Entities
{
	internal class Washer
	{
		public string Brand { get; }
		public uint CapacityKg { get; }
		public float KWh { get; }

		public Washer(string brand, uint capacityKg, float kWh)
		{
			Brand = brand;
			CapacityKg = capacityKg;
			KWh = kWh;
		}

		public void StartWash()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} starts washing.");			
		}
		public void StopWash()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} stops washing.");			
		}
		public void PrintWashEnergy()
		{
			Console.WriteLine($"{Brand} {this.GetType().Name.ToLower()} uses {KWh} kWh per wash.");			
		}
	}	
}