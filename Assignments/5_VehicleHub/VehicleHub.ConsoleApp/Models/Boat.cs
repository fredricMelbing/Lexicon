namespace VehicleHub.ConsoleApp.Models
{
	internal class Boat : Vehicle
	{
		public uint Lenght { get; set; }

		public Boat(string regNum, string color, uint wheels, uint lenght)
			: base(regNum, color, wheels)
		{
			Lenght = lenght;
		}

		public override string GetInfo()
		{
			return $"{base.GetInfo()}, Lenght: {Lenght}";
		}
	}
}
