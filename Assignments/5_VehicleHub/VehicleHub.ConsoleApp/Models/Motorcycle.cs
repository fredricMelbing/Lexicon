namespace VehicleHub.ConsoleApp.Models
{
	internal class Motorcycle : Vehicle
	{
		public uint CylinderVolume { get; set; }		

		public Motorcycle(string regNum, string color, uint wheels, uint cylinderVolume)
			: base(regNum, color, wheels)
		{
			CylinderVolume = cylinderVolume;
		}
		
		
		public override string GetInfo()
		{
			return $"{base.GetInfo()}, Cylinder Volume: {CylinderVolume}";
		}
	}
}
