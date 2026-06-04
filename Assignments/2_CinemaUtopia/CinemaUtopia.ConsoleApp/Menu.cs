namespace CinemaUtopia.ConsoleApp
{
	internal class Menu
	{
		public void Run()
		{
			MainMenu();
		}
		//Main menu with user choices
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
				Console.WriteLine("3. Write Something!");
				Console.WriteLine("4. Write Sentence with at least 3 Words");
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
				case "3":
					WriteSomething();
					return true;
				case "4":
					WriteSentence();
					return true;
				default:
					Console.Clear();
					Console.WriteLine("Invalid option. Please try again.");
					return true;
			}
		}
		//Buy Tickets user will be asked to enter how many tickets to buy
		//and then enter age for each ticket,
		//then printout price for each ticket and total price for all tickets.
		private void BuyTickets()
		{
			Console.Clear();
			Console.WriteLine("How many tickets would you like to buy?");
			string input = Console.ReadLine() ?? string.Empty;
			if (uint.TryParse(input, out uint numberOfTickets))
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
			DisplaySoldTickets(); //Summerize and printout total price for all tickets.
		}
		//Create ticket based on age input.
		private void CreateTicket(uint age)
		{
			if (age < 5 || age > 100)
				new FreeTicket { };
			else if (age < 20)
				new JuvenileTicket { };
			else if (age > 64)
				new SeniorTicket { };
			else
				new StandardTicket { };
		}
		//Display all sold tickets and total price for all tickets.
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
		//User will be asked to enter something and then printout what they entered 10 times in one line.
		private void WriteSomething()
		{
			Console.Clear();
			Console.WriteLine("Please enter something:");
			string input = Console.ReadLine() ?? string.Empty;			
			string output = string.Empty;
			for (int i = 0; i < 10; i++)
			{
				if(i!=9)
					output += $"{i+1}. {input}, ";
				else
					output += $"{i + 1}. {input}";
			}			
			Console.WriteLine(output);
			Console.WriteLine();
			Console.WriteLine("Press any key to return to the main menu...");
			Console.ReadKey();
			Console.Clear();
		}
		//User will be asked to enter a sentence with at least 3 words and then printout the 3:e word in the sentence.
		//empty spaces should not be counted as words.
		private void WriteSentence()
		{
			Console.Clear();
			Console.WriteLine("Please enter a sentence with at least 3 words:");
			string input = Console.ReadLine() ?? string.Empty;
			string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length >= 3)
				Console.WriteLine($"The 3rd word in your sentence is: {words[2]}");
			else
				Console.WriteLine("Too few words in sentence.");
			Console.WriteLine("Press any key to return to the main menu...");
			Console.ReadKey();
			Console.Clear();
		}
	}
}