using PersonnelRegister.Core.Interface;
using PersonnelRegister.Domain.Entities;


namespace PersonnelRegister.Core.Service
{
	public class MenyService : IMenyService
	{
		public MenyService()
		{

		}		
		public void Run()
		{
			List<Personnel> personnel = new List<Personnel>();
			bool running = true;
			while (running)
			{
				int menyOptions = ShowMenu();
				var ob = MenyChoice(menyOptions, personnel);
				if (ob is Personnel newEmployee)
					personnel.Add(newEmployee);
				else if (ob is bool exit && !exit)
					running = false;
			}
		}
		private int ShowMenu()
		{
			List<string> list = new List<string> {
				"Create new Emploeey",
				"Show Employee",
				"Exit"
			};
			for (int i = 0; i < list.Count; i++)
			{
				Console.WriteLine($"[{i + 1}] {list[i]}");
			}
			return list.Count;
		}
		private object MenyChoice(int caseNuber, List<Personnel> personnel)
		{
			int choice = 0;
			if (!int.TryParse(Console.ReadLine(), out choice) || choice <= 0 || choice > caseNuber)
				Console.WriteLine("Invalid input. Please enter a valid input.");

			switch (choice)
			{
				case 1:
					Console.Clear();
					return CreateNewEmployee();
				case 2:
					Console.Clear();
					ShowEmployee(personnel);
					Console.ReadKey();
					return true;
				case 3:
					Console.WriteLine("Exit");
					return false;
				default:
					Console.Clear();
					Console.WriteLine("Invalid choice. Please try again.");
					return true;
			}
		}
		private Personnel CreateNewEmployee()
		{
			Console.WriteLine("Enter first name:");
			string firstName = Console.ReadLine();
			Console.WriteLine("Enter last name:");
			string lastName = Console.ReadLine();
			Console.WriteLine("Enter salary:");
			decimal salary;			
			while (!decimal.TryParse(Console.ReadLine(), out salary) || salary < 0)
			{
				Console.Clear();
				Console.WriteLine("Invalid input. Please enter a valid salary:");
			}			
			Personnel newEmployee = new Personnel
			{
				FirstName = firstName,
				LastName = lastName,
				Salary = salary
			};
			Console.Clear();
			return newEmployee;			
		}
		private void ShowEmployee(List<Personnel> personnel)
		{
			foreach (var employee in personnel)
			{
				Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}, Salary: {employee.Salary}");
			}
		}
	}
}
