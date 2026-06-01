using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoreConsole.ConsoleApp.PDF
{
	internal class PDF
	{
		internal void Questions()
		{			
			Console.WriteLine("Hur fungerar stacken och heapen? Förklara gärna med exempel eller skiss på dess grundläggande funktion");
			Console.WriteLine();
			Console.WriteLine("Stacken är en supersnabb, strikt strukturerad minnesyta. Där lagras Value Types och Pointers. " +
				"Den fungerar enligt LIFO-principen (Last In, First Out).");			
			Console.WriteLine("Heapen är en mer flexibel minnesyta där objekt lagras. Den används för att lagra Reference Types. " +
				"Man nåer objekten via Pointers. Minnet rensas automatiskt av GC (Garbage Collector).");
			Readfromfile();			

			Console.WriteLine();			
			Console.WriteLine("Vad är Value Types respektive Reference Types och vad skiljer dem åt?");
			Console.WriteLine("Value Types lagras på stacken och innehåller direkt värdet. " +
				"Reference Types lagras på heapen och innehåller en referens till platsen där objektet ligger.");

			Console.WriteLine();			
			Console.WriteLine($"Följande metoder (se metoder \"ReturnValue()\", \"ReturnValue2()\") genererar olika svar. " +
				$"Den första returnerar {ReturnValue()}, den andra returnerar {ReturnValue2()}, varför?");
			Console.WriteLine("Första metoden returnerar 3 eftersom den arbetar med value types (stack), " +
				"medan andra metoden returnerar 4 eftersom den arbetar med reference types (heap).");

			Console.WriteLine();
			Console.WriteLine("Starta programmet");
			Console.ReadKey();
		}
		private void Readfromfile()
		{
			try
			{
				string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\PDF\Design_Stack_Heap.txt");
				Console.WriteLine();

				foreach (string line in File.ReadLines(path))
				{
					Console.WriteLine(line);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
		private int ReturnValue()
		{
			int x = new int();
			x = 3;
			int y = new int();
			y = x;
			y = 4;
			return x;
		}
		private int ReturnValue2()
		{
			Myint x = new Myint();
			x.MyValue = 3;
			Myint y = new Myint();
			y = x;
			y.MyValue = 4;
			return x.MyValue;
		}
		private class Myint
		{
			internal int MyValue { get; set; }
		}
	}	
}