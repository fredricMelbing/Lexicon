namespace CinemaUtopia.ConsoleApp
{
	internal class Menu
	{	
		public void Run()
		{
			MainMenu();
		}
		private void MainMenu()
		{
			bool runProgram = true;
			do
			{
				Console.WriteLine("Welcome to Cinema Utopia's Main Menu!");
				Console.WriteLine("Please select an option:");
				Console.WriteLine("0. Exit");
				Console.WriteLine("1. Buy Tickets");
				Console.WriteLine("2. Display Sold Tickets");
				string userInput = Console.ReadLine() ?? string.Empty;
				if (!MainMenuCase(userInput))
					runProgram = false;
			} while (runProgram);
		}
		private bool MainMenuCase(string userInput)
		{
			switch (userInput)
			{
				case "0":
					Console.WriteLine("Thank you for using Cinema Utopia!");
					return false;
				case "1":
					BuyTickets();
					return true;
				case "2":
					DisplaySoldTickets();
					return true;
				default:
					Console.WriteLine("Invalid option. Please try again.");
					Thread.Sleep(1000);
					Console.Clear();
					//MainMenu(); //TODO: Extra 4. remove do while and run method instead.
					return true;
			}
		}		
		private void BuyTickets()
		{
			Console.Clear();
			Console.WriteLine("How many tickets would you like to buy?");
			string input = Console.ReadLine() ?? string.Empty;
			if (int.TryParse(input, out int numberOfTickets) && numberOfTickets > 0)
			{
				for (int i = 0; i < numberOfTickets; i++)
				{
					Console.WriteLine($"Please enter the age for ticket {i + 1}:");
					string ageInput = Console.ReadLine() ?? string.Empty;
					if (uint.TryParse(ageInput, out uint age))
						CreateTicket(age);
					else
					{
						Console.WriteLine("Invalid age input. Please enter a valid number.");
						i--; // Decrement to retry the current ticket
					}
				}
				Console.Clear();
			}
			else
			{
				Console.WriteLine("Invalid number of tickets. Please enter a valid number.");
				Console.WriteLine("Press any key to return to the main menu...");
				Console.ReadKey();
				Console.Clear();
			}			
			DisplaySoldTickets();
		}
		private void CreateTicket(uint age)
		{
			if (age < 20)
				new JuvenileTicket { };
			else if (age > 64)
				new SeniorTicket { };
			else
				new StandardTicket { };
		}
		private void DisplaySoldTickets()
		{
			Console.Clear();
			if (Ticket.soldTickets.Count == 0)
				Console.WriteLine("No tickets sold yet.");
			else
			{
				Console.WriteLine("Sold Tickets:");
				foreach (Type type in Ticket.soldTickets.Select(t => t.GetType()).Distinct())
				{
					int count = Ticket.soldTickets.Count(t => t.GetType() == type);
					int ticketPrice = Ticket.soldTickets.Where(t => t.GetType() == type).Select(t => t.Price).FirstOrDefault();
					Console.WriteLine($"{type.Name}: {count} ticket(s), {ticketPrice} kr, total: {count * ticketPrice} kr");
				}
				Console.WriteLine($"Total price for all sold tickets: {Ticket.soldTickets.Sum(t => t.Price)} kr");				
				Console.WriteLine("Press any key to return to the main menu...");
				Console.ReadKey();
				Console.Clear();
			}
		}
	}
}