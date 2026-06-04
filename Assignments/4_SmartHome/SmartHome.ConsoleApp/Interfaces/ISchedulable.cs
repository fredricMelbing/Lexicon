namespace SmartHome.ConsoleApp.Interfaces
{
	internal interface ISchedulable
	{
		DateTime NextRun { get; set; }
		void Schedule(DateTime time);
	}
}
