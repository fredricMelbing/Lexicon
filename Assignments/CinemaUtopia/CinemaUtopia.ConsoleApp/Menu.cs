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
				default:
					Console.WriteLine("Invalid option. Please try again.");
					Thread.Sleep(1000);
					Console.Clear();
					//MainMenu(); //TODO: Extra 4. remove do while and run method instead.
					return true;
			}
		}
	}
}