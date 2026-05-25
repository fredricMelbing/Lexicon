namespace CinemaUtopia.ConsoleApp
{
	internal abstract class Ticket
	{
		public static List<Ticket> soldTickets { get; } = new List<Ticket>();
		public abstract int Price { get;}
		protected Ticket()
		{
			soldTickets.Add(this);
			//PrintPrice();
		}
		public virtual void PrintPrice()
		{

		}
	}
	class JuvenileTicket : Ticket
	{
		public override int Price { get; } = 80;
		public override void PrintPrice()
		{
			Console.WriteLine($"The price for ticket {this.GetType().Name} is: {this.Price} kr");
		}		
	}
	class SeniorTicket : Ticket
	{
		public override int Price { get; } = 90;
		public override void PrintPrice()
		{
			Console.WriteLine($"The price for ticket {this.GetType().Name} is: {this.Price} kr");
		}
	}
	class StandardTicket : Ticket
	{
		public override int Price { get; } = 120;
		public override void PrintPrice()
		{
			Console.WriteLine($"The price for ticket {this.GetType().Name} is: {this.Price} kr");
		}
	}
	class FreeTicket : Ticket
	{
		public override int Price { get; } = 0;
		public override void PrintPrice()
		{
			Console.WriteLine($"The price for ticket {this.GetType().Name} is: {this.Price} kr");
		}
	}	
}