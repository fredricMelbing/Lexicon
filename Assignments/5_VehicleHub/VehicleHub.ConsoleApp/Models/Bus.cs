namespace VehicleHub.ConsoleApp.Models
{
	internal class Bus : Vehicle
	{
		public uint NumberOfSeats { get; set; }

		public Bus(string regNum, string color, uint wheels, uint numberOfSeats)
			: base(regNum, color, wheels)
		{
			NumberOfSeats = numberOfSeats;
		}

		public override string GetInfo()
		{
			return $"{base.GetInfo()}, Seats: {NumberOfSeats}";
		}
	}
}
