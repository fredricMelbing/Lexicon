namespace SmartHome.ConsoleApp.Entities
{
	internal class SmartLamp : Appliance
	{
		public double Brightness { get; set; }
		public SmartLamp(string brand, string room, bool isOn, double brightness, double kWh) : base(brand, room, isOn, kWh)
		{
			Brightness = brightness;
		}

		public override void TurnOn()
		{
			Console.WriteLine($"{this.GetType().Name.ToLower()} is now ON. Brightness: {Brightness}%");			
		}
	}
}
