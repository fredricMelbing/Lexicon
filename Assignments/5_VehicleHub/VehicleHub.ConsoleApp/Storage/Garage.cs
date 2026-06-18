using System.Collections;
using VehicleHub.ConsoleApp.Interfaces;

namespace VehicleHub.ConsoleApp.Storage
{
	internal class Garage<T> : IEnumerable<T> where T : IVehicle
	{
		private T?[] _vehicles;
		public int Capacity { get; }
		public int Count => System.Linq.Enumerable.Count(this);


		public Garage(int capacity)
		{
			if (capacity <= 0) throw new ArgumentException("Capacity must be greater than zero.");			

			Capacity = capacity;
			_vehicles = new T[capacity];
		}


		public bool Park(T vehicle)
		{
			if (vehicle == null) return false;

			for (int i = 0; i < _vehicles.Length; i++)
			{
				if (_vehicles[i] == null)
				{
					_vehicles[i] = vehicle;
					return true;
				}
			}
			return false;
		}

		public bool Remove(string regNum)
		{
			if (string.IsNullOrWhiteSpace(regNum)) return false;

			for (int i = 0; i < _vehicles.Length; i++)
			{				
				if (_vehicles[i]?.RegistrationNumber.Equals(regNum, StringComparison.OrdinalIgnoreCase) == true)
				{
					_vehicles[i] = default;
					return true;
				}
			}
			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{			
			foreach (var vehicle in _vehicles)
			{
				if (vehicle != null)
					yield return vehicle;
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}