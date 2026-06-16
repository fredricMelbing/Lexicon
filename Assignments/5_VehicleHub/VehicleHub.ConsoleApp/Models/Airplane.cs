namespace VehicleHub.ConsoleApp.Models
{
	internal class Airplane : Vehicle
	{
		public uint NumberOfEngines { get; set; }

		public Airplane(string regNum, string color, uint wheels, uint numberOfEngines)
			: base(regNum, color, wheels)
		{
			NumberOfEngines = numberOfEngines;
		}

		public override string GetInfo()
		{			
			return $"{base.GetInfo()}, Engines: {NumberOfEngines}";
		}
	}
}
