using StoreConsole.ConsoleApp.PDF;
using StoreConsole.DemoClasses;
using StoreConsole.Helpers;
using StoreConsole.StackAndHeap;
using System.Text;

namespace StoreConsole;

internal class Program
{
	// Dictionary: snabb uppslagning av produkter via produktkod (key = kod, value = produkt)
	static Dictionary<string, Product> products = new Dictionary<string, Product>();

	// List: enkel logg över vad som hänt i programmet — ordnad och växer dynamiskt
	static List<string> logMessages = new List<string>();

	// Queue: FIFO — kunder betjänas i den ordning de ställde sig i kön
	static Queue<Customer> customerQueue = new Queue<Customer>();

	// Stack: LIFO — används för att kunna ångra den senaste försäljningen
	static Stack<Sale> saleHistory = new Stack<Sale>();

	static string ReadLine => Console.ReadLine() ?? string.Empty;

	static void Main(string[] args)
	{		
		PDF pdf = new PDF();
		pdf.Questions();

		SeedProducts();
		bool running = true;
		do
		{
			PrintMenu();

			Console.Write("Välj: ");
			string choice = ReadLine;

			Console.WriteLine();

			switch (choice)
			{
				case MenuConstants.ShowProducts:
					PrintProducts();
					break;

				case MenuConstants.FindProduct:
					FindProduct();
					break;

				case MenuConstants.AddProduct:
					AddProduct();
					break;

				case MenuConstants.ChangeStock:
					ChangeStock();
					break;

				case MenuConstants.GetBetterPrice:
					Console.Write("Ange produktkod: ");
					GetPriceBetter(ReadLine.ToUpper());
					break;

				case MenuConstants.AddCustomerToQueue:
					AddCustomerToQueue();
					break;

				case MenuConstants.ServeNextCustomer:
					ServeNextCustomer();
					break;

				case MenuConstants.PrintCustomerQueue:
					PrintCustomerQueue();
					break;

				case MenuConstants.SellProduct:
					SellProduct();
					break;

				case MenuConstants.UndoLastSale:
					UndoLastSale();
					break;

				case MenuConstants.PrintLog:
					PrintLog();
					break;

				case MenuConstants.ArrayLab:
					ArrayLab();
					break;

				case MenuConstants.ListLab:
					ListLab();
					break;

				case MenuConstants.ReverseTextLab:
					ReverseTextLab();
					break;

				case MenuConstants.WordCountLab:
					WordCountLab();
					break;

				case MenuConstants.ParenthesesLab:
					ParenthesesLab();
					break;

				case MenuConstants.MemoryLab:
					MemoryLab();
					break;

				case MenuConstants.RecursionLab:
					RecursionLab();
					break;

				case MenuConstants.SaveLogToFile:
					SaveLogToFile();
					break;

				case MenuConstants.Exit:
					running = false;
					break;

				default:
					Console.WriteLine("Felaktigt val.");
					break;
			}

			Console.WriteLine();
			Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
			Console.ReadKey();
			Console.Clear();
		}
		while (running);
	}
	static void PrintMenu()
	{
		Console.WriteLine(MenuConstants.Title);
		Console.WriteLine();

		foreach (MenuItem item in MenuConstants.Items)
		{
			Console.WriteLine(item);
		}

		Console.WriteLine();
	}
	#region Dictionary

	// ============================================================
	// DEL 1 - PRODUKTER OCH DICTIONARY
	// ============================================================

	static void SeedProducts()
	{
		products["KAF"] = new Product("KAF", "Kaffe", 15.00m, 50);
		products["TE"] = new Product("TE", "Te", 12.00m, 30);
		products["BUL"] = new Product("BUL", "Bullar", 18.00m, 20);
		products["MCK"] = new Product("MCK", "Mjölk", 35.00m, 15);
		products["GOD"] = new Product("GOD", "Godis", 25.00m, 25);
		products["KOL"] = new Product("KOL", "Kola", 20.00m, 40);
		products["VIT"] = new Product("VIT", "Vitlök", 10.00m, 35);
		products["LÖK"] = new Product("LÖK", "Lök", 8.00m, 45);
		products["TOM"] = new Product("TOM", "Tomat", 12.00m, 30);
		products["GUR"] = new Product("GUR", "Gurka", 14.00m, 25);
	}
	static void PrintProducts()
	{
		Console.WriteLine("=== Produkter ===");

		//Prints all products in the products dictionary and the total stock saldo/product.		
		products.ToList().ForEach(p => Console.WriteLine($"{p.Value}: Total lagervärde: {p.Value.Price * p.Value.Stock}"));
		Console.WriteLine();

		//Prints total value of all products in stock.
		Console.WriteLine($"=== Totalt lagervärde: {products.Sum(pd => pd.Value.Price * pd.Value.Stock)} ===");

		Console.WriteLine();
		// Varför passar Dictionary bra för ett produktregister?
		Console.WriteLine("Svar: Dictionary passar perfekt för ett produktregister eftersom " +
			"den ger blixtsnabb sökning via unika nycklar som produktnummer eller streckkoder. " +
			"Till skillnad från en vanlig lista behöver programmet inte leta igenom hela registret rad för rad");
	}
	static void FindProduct()
	{
		string input = InputHelpers.ReadString("Ange produktkod: ");

		if (products.TryGetValue(input.ToUpper(), out Product? product))
			Console.WriteLine($"Produkt hittad: {product}");
		else
			Console.WriteLine("Produktkod hittades inte.");

		// Varför är TryGetValue bättre än att skriva products[code] direkt?
		Console.WriteLine("Svar: För att unvika att programmet krashar om produkten inte finns.");
	}
	static void AddProduct()
	{
		string code;
		do
		{
			code = InputHelpers.ReadString("Ange produktkod: ").ToUpper();
			if (!products.ContainsKey(code))
				break;
			Console.WriteLine("Produktkod finns redan. Försök igen.");
		} while (true);

		string name = InputHelpers.ReadString("Ange produktnamn: ");
		decimal price = InputHelpers.ReadDecimal("Ange pris: ");
		int stock = InputHelpers.ReadInt("Ange lagersaldo: ");

		//Adding new product to the products dictionary.
		products[code] = new Product(code, name, price, stock);
		//Adding log entry for the new product.
		logMessages.Add($"Produkt tillagd: {products[code]}");

		Console.WriteLine("Produkt tillagd.");

		// Vad är nyckeln och vad är värdet i products?
		Console.WriteLine("Svar: nyckeln är Hashing av stringen som skapar en Array där man pekar vart objektet är lagrat som är värdet.");
	}
	static void ChangeStock()
	{
		//Changes the stock of a product in the products dictionary.
		if (products.TryGetValue(InputHelpers.ReadString("Ange produktkod: ").ToUpper(), out Product? product))
		{
			product.Stock = InputHelpers.ReadInt("Ange lagersaldo: ");
			logMessages.Add($"Lagersaldo ändrat: {product}");
			Console.WriteLine("Lagersaldo uppdaterat.");
		}
		else
			Console.WriteLine("Produktkod hittades inte.");
	}
	static decimal GetPriceBad(string code)
	{
		if (code == "KAF")
		{
			return 15;
		}
		else if (code == "TE")
		{
			return 12;
		}
		else if (code == "BUL")
		{
			return 18;
		}
		else if (code == "MCK")
		{
			return 35;
		}
		else if (code == "GOD")
		{
			return 25;
		}
		else
		{
			return -1;
		}
	}
	static decimal GetPriceBetter(string code)
	{

		// Jämför sedan de två metoderna — vad händer om du behöver lägga till
		// en femte produkt? Vilken metod är enklare att utöka?
		// Det är mycket enklare att utöka GetPriceBetter

		// Varför är Dictionary-lösningen bättre än många if/else-satser?
		Console.WriteLine("Svar: Bättre prestanda och lättare att underhålla");

		Dictionary<string, decimal> priceList = new Dictionary<string, decimal>
		{
			{ "KAF", 15 },
			{ "TE", 12 },
			{ "BUL", 18 },
			{ "MCK", 35 },
			{ "GOD", 25 }
		};
		if (priceList.TryGetValue(code.ToUpper(), out decimal price))
			return price;
		else
			return -1;
	}
	#endregion

	#region Queue

	// ============================================================
	// DEL 2 - QUEUE
	// ============================================================

	static void AddCustomerToQueue()
	{
		string name = InputHelpers.ReadString("Ange kundens namn: ");
		Customer customer = new Customer(name);

		//Lägger till Kunden längst ner i listan/kön.
		customerQueue.Enqueue(customer);
		Console.WriteLine($"Kunden {customer.Name} har lagts till i kön. Plats i kön: {customerQueue.Count}");
		logMessages.Add($"Kund tillagd: {customer.Name}");

		// Vad betyder FIFO?
		Console.WriteLine("FIFO betyder First In, First Out");
	}
	static void ServeNextCustomer()
	{
		if (customerQueue.Count > 0)
		{
			Customer nextCustomer = customerQueue.Dequeue();
			Console.WriteLine($"Kunden {nextCustomer.Name} har blivit serverad.");
			logMessages.Add($"Kund serverad: {nextCustomer.Name}");
		}
		else
			Console.WriteLine("Ingen kund i kön.");

		// Varför passar Queue bättre än Stack för en kundkö?
		Console.WriteLine("Queue passar bättre då den följer principen om first in, first out (FIFO) medans stacken följer principen om last in, first out (LIFO)");
	}
	static void PrintCustomerQueue()
	{
		Console.WriteLine("=== Kundkö ===");

		if (customerQueue.Count > 0)
		{
			customerQueue.ToList().ForEach(c =>
			{
				Console.WriteLine($"{customerQueue.ToList().IndexOf(c) + 1}. {c.Name} ({c.AddedAt})");
			});
		}
		else
			Console.WriteLine("Ingen kund i kön.");
	}
	#endregion

	#region Stack

	// ============================================================
	// DEL 3 - STACK OCH FÖRSÄLJNING
	// ============================================================

	static void SellProduct()
	{
		// Bestäm om kunden ska tas bort från kön efter köp eller inte.
		// Motivera ditt val i kommentar.

		// Jag uppfattar att man ska kunna sälja flera varor eller ångra köp.
		// Därefter ska Customer kunna tas bort med hjälp utav ServeNextCustomer().


		if (customerQueue.Count > 0)
		{
			Customer customer = customerQueue.Peek();
			string input = InputHelpers.ReadString("Ange produktkod: ");
			if (products.TryGetValue(input.ToUpper(), out Product? product) && product.Stock > 0)
			{
				product.Stock--;

				Sale sale = new Sale(product.Code, product.Name, product.Price, customer.Name);
				saleHistory.Push(sale);
				logMessages.Add(sale.ToString());
			}
			else if (product is null)
				Console.WriteLine($"Produkt finns ej");
			else
				Console.WriteLine($"Lagersaldo är 0");
		}
		else
			Console.WriteLine("Ingen kund i kön.");

		// Varför sparar vi försäljningar i en Stack?
		Console.WriteLine("För att kunna \"ångra\" senaste köp LIFO: Last In, First Out.");
	}

	static void UndoLastSale()
	{
		if (saleHistory.Count > 0)
		{
			Sale sale = saleHistory.Pop();

			if (products.TryGetValue(sale.ProductCode.ToUpper(), out Product? product))
			{
				product.Stock++;
				logMessages.Add("Ångrat köp: " + sale.ToString());
			}
			else
				Console.WriteLine("Produkten är borttagen från försälning men är ej inlagd i lager då den inte existerar");
		}
		else
			Console.WriteLine("Inga sålda varor.");

		// Vad betyder LIFO?
		Console.WriteLine("LIFO: Last In, First Out.");
	}

	static void ReverseTextLab()
	{
		Console.WriteLine("=== Stack-labb: vänd text ===");

		Stack<char> charStack = new Stack<char>();
		InputHelpers.ReadString("Ange en text: ").ToList().ForEach(c => charStack.Push(c));
		Console.WriteLine(new string(charStack.ToArray()));
	}
	#endregion

	#region List

	// ============================================================
	// DEL 4 - LIST
	// ============================================================

	static void PrintLog()
	{
		Console.WriteLine("=== Logg ===");

		if (logMessages.Count > 0)
			logMessages.ForEach(logMessage => Console.WriteLine(logMessage));
		else
			Console.WriteLine("Inga loggmeddelanden finns.");

		// Varför passar List bra för loggmeddelanden?
		Console.WriteLine("Det går fort att lägga till ett element och det läggs i kronologisk ordning.");
	}
	static void ListLab()
	{
		Console.WriteLine("=== List-labb ===");

		List<string> shoppingList = new List<string>();

		PrintListInfo(shoppingList, "Start");

		shoppingList.Add("Mjölk");
		PrintListInfo(shoppingList, "Efter Mjölk");

		shoppingList.Add("Bröd");
		PrintListInfo(shoppingList, "Efter Bröd");

		shoppingList.Add("Smör");
		PrintListInfo(shoppingList, "Efter Smör");

		shoppingList.Add("Ost");
		PrintListInfo(shoppingList, "Efter Ost");

		shoppingList.Add("Yoghurt");
		PrintListInfo(shoppingList, "Efter Yoghurt");

		shoppingList.Remove("Smör");
		PrintListInfo(shoppingList, "Efter Remove");

		new List<string> { "Nötter", "Mjöl", "Kaffe", "Te" }.ForEach(item => shoppingList.Add(item));
		shoppingList.ForEach(item => Console.WriteLine(item));

		// Vad betyder Count?
		Console.WriteLine("Det betyder antalet element i listan.");

		// Vad betyder Capacity?
		Console.WriteLine("Det betyder antalet element som listan kan innehålla utan att behöva allokeras om och är alltid större än eller lika med Count.");

		// Varför ökar inte Capacity med exakt 1 varje gång?
		Console.WriteLine("Det tar tid att allokeras om och är en prestandaförbättring så att det istället dubbleras när det behövs.");

		// Minskar Capacity automatiskt när element tas bort?
		Console.WriteLine("Nej, Capacity minskar inte automatiskt när element tas bort men man kan göra det manuellt med TrimExcess().");
	}
	static void PrintListInfo(List<string> list, string message)
	{
		Console.WriteLine($"{message}: Count = {list.Count}, Capacity = {list.Capacity}");
	}

	#endregion

	#region Array

	// ============================================================
	// DEL 5 - ARRAY
	// ============================================================

	static void ArrayLab()
	{
		Console.WriteLine("=== Array-labb ===");

		string[] weekdays = ["Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag"];

		// Skriv ut alla veckodagar med en for-loop.
		// Tips: använd weekdays.Length för att veta hur många element det finns.
		for (int i = 0; i < weekdays.Length; i++)
			Console.WriteLine(weekdays[i]);

		// Skriv ut alla veckodagar med foreach.		
		//weekdays.ToList().ForEach(day => Console.WriteLine(day));		
		foreach (string day in weekdays)
			Console.WriteLine(day);

		// När passar en array bättre än en List?
		Console.WriteLine("Det passar när antalet element är känt från början och inte förändras mycket.");
		//Det finns något som heter ArrayList som är en flexibel version av en array. 
		//Microsoft själva rekommenderar däremot inte att använda ArraList då den kan ha sämre prestanda och hänvisar till List<T> istället.
		//https://learn.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=net-10.0

		// Vad händer om du försöker skriva weekdays[5]?
		Console.WriteLine("Eftersom index 5 inte finns i arrayen så krashar programmet och skickar" +
			" \"System.IndexOutOfRangeException: 'Index was outside the bounds of the array.'\".");

		// Varför måste arrayens storlek anges från början?
		Console.WriteLine("Eftersom arrayens storlek måste definieras vid skapandet och inte kan ändras senare. " +
			"En array kräver ett sammanhängande minnesblock på heapen.");
	}

	#endregion

	#region Blandat_Stack_Heap_mm

	// ============================================================
	// DEL 6 - DICTIONARY SOM RÄKNARE
	// ============================================================

	static void WordCountLab()
	{
		Console.WriteLine("=== Dictionary-labb: räkna ord ===");

		Console.WriteLine("Skriv en mening:");
		string text = Console.ReadLine() ?? string.Empty;

		Dictionary<string, int> wordCounts = CountWords(text);

		Console.WriteLine("Resultat:");

		foreach (KeyValuePair<string, int> pair in wordCounts)
		{
			Console.WriteLine($"{pair.Key}: {pair.Value}");
		}

		// Varför passar Dictionary bra när vi ska räkna ord?
		Console.WriteLine("För att ordet kan användas som nyckel och antalet förekomster som värde.");
	}
	static Dictionary<string, int> CountWords(string text)
	{
		Dictionary<string, int> wordCounts = new Dictionary<string, int>();

		wordCounts = text.Split(new char[] { ' ', '.', ',', '!', '?', ':', ';' }, StringSplitOptions.RemoveEmptyEntries)
			.GroupBy(word => word.ToLower())
			.ToDictionary(group => group.Key, group => group.Count());

		// Vad är nyckeln och vad är värdet i wordCounts?
		Console.WriteLine("Nyckeln är ordet och värdet är antalet gånger det förekommer.");
		return wordCounts;
	}

	// ============================================================
	// DEL 7 - PARENTESKONTROLL - Använd lämpliga datastrukturer
	// ============================================================

	static void ParenthesesLab()
	{
		Console.WriteLine("=== Kontrollera parenteser ===");

		// Testfall att prova:
		// ([{}])                         true
		// ({)}                           false
		// List<int> lista = new();       true
		// (]                             false
		// ((()))                         true
		// (()                            false
		// (                              false
		// )                              false
		Console.WriteLine("Skriv en kodrad eller parentessträng:");
		string input = Console.ReadLine() ?? string.Empty;

		//string[] testCases = new string[]
		//{
		//	"([{}])",
		//	"({)}",
		//	"List<int> lista = new();",
		//	"(]",
		//	"((()))",
		//	"(()",
		//	"(",
		//	")"
		//};
		//bool[] expectedResults = new bool[] { true, false, true, false, true, false, false, false };

		//for (int i = 0; i < testCases.Length; i++)
		//{
		//	bool check = CheckParentheses(testCases[i]);
		//	Console.WriteLine($"{testCases[i]} -> {check} == {expectedResults[i]}");
		//}

		bool isCorrect = CheckParentheses(input);

		if (isCorrect)
		{
			Console.WriteLine("Strängen är välformad.");
		}
		else
		{
			Console.WriteLine("Strängen är INTE välformad.");
		}
	}
	static bool CheckParentheses(string text)
	{
		// Använd en Stack<char> och en Dictionary<char, char>.
		// Tips Dictionary:
		// Låt dictionaryn mappa varje stängande parentes till sin matchande öppnare.
		// Det gör matchningskontrollen till en enkel uppslagning istället för flera if-satser.

		// Tips Stack:
		// Stacken håller reda på vilka öppnare du sett men ännu inte stängt.
		// Tänk på vad LIFO innebär här — varför är det precis rätt egenskap för det här problemet?

		char[] chars = text.ToCharArray();
		Stack<char> stack = new Stack<char>();
		Dictionary<char, char> parenthesesPairs = new Dictionary<char, char>
		{
			{ '(', ')' },
			{ '[', ']' },
			{ '{', '}' }
		};

		foreach (char c in chars)
		{
			if (parenthesesPairs.ContainsKey(c))
				stack.Push(c);
			else if (parenthesesPairs.ContainsValue(c) && stack.Count > 0 && parenthesesPairs[stack.Peek()] == c)
				stack.Pop();
			else if (parenthesesPairs.ContainsValue(c) && (stack.Count == 0 || parenthesesPairs[stack.Peek()] != c))
				return false;
		}
		if (stack.Count == 0)
			return true;

		// Varför är Dictionary + Stack bättre än bara Stack med if/else för matchningen?
		Console.WriteLine("Dictionary tillåter snabb uppslagning av matchande parenteser, " +
			"vilket gör koden mer läsbar och underhållbar jämfört med flera if/else-satser.");

		return false;
	}

	// ============================================================
	// DEL 8 - STACKEN OCH HEAPEN
	// ============================================================

	static void MemoryLab()
	{
		Console.WriteLine("=== Value type: int ===");

		int number1 = 10;
		int number2 = number1;

		number2 = 99;

		Console.WriteLine($"number1: {number1}");
		Console.WriteLine($"number2: {number2}");

		Console.WriteLine();
		Console.WriteLine("=== Value type: struct ===");

		ScoreValue score1 = new ScoreValue(10);
		ScoreValue score2 = score1;

		score2.Points = 99;

		Console.WriteLine($"score1.Points: {score1.Points}");
		Console.WriteLine($"score2.Points: {score2.Points}");

		Console.WriteLine();
		Console.WriteLine("=== Reference type: class ===");

		ScoreReference refScore1 = new ScoreReference(10);
		ScoreReference refScore2 = refScore1;

		refScore2.Points = 99;

		Console.WriteLine($"refScore1.Points: {refScore1.Points}");
		Console.WriteLine($"refScore2.Points: {refScore2.Points}");

		Console.WriteLine();
		Console.WriteLine("=== Reference type: Product ===");

		Product product1 = new Product("KAF", "Kaffe", 15, 20);
		Product product2 = product1;

		product2.Stock = 0;

		Console.WriteLine(product1);
		Console.WriteLine(product2);

		// Varför ändras inte number1 när number2 ändras?
		Console.WriteLine("För att int är en valuetype, vilket innebär att varje variabel har sin egen plats i minnet.");

		// Varför ändras inte score1.Points när score2.Points ändras?
		Console.WriteLine("För att struct är en valuetype, vilket innebär att varje variabel har sin egen plats i minnet.");

		// Varför ändras product1.Stock när product2.Stock ändras?
		Console.WriteLine("För att Product är en reference type, vilket innebär att båda variablerna pekar på samma objekt i minnet.");

		// Är Product en value type eller reference type?
		Console.WriteLine("Reference type");

		// Vad ligger på heapen i Product-exemplet?
		Console.WriteLine("Objectet kommer att ligg på heapen.");

		// Vad innebär det att två variabler kan peka på samma objekt?
		Console.WriteLine("Det innebär att båda variablerna hänvisar till samma minnesplats där objektet är lagrat.");

		// Vad är skillnaden mellan stacken i minnet och Stack<T> som datastruktur?		
		Console.WriteLine("Skillnaden är att stacken i minnet är en fysisk struktur där metoder och variabler lagras, " +
			"medan Stack<T> är en datastruktur i kod som hanterar element i en LIFO-kö. " +
			"Stack<T> lagrar själva datastrukturen och alla dess element på heapen i minnet, inte på den fysiska stacken.");
	}

	#endregion

	#region ExtraUppgifter


	// ============================================================
	// DEL 9 - REKURSION OCH ITERATION EXTRA om tid finns
	// ============================================================

	static void RecursionLab()
	{
		Console.WriteLine("=== Rekursion och iteration ===");

		Console.Write("Ange n: ");

		if (!int.TryParse(ReadLine, out int n))
		{
			Console.WriteLine("Du måste skriva ett heltal.");
			return;
		}

		if (n <= 0)
		{
			Console.WriteLine("n måste vara större än 0.");
			return;
		}

		Console.WriteLine();
		Console.WriteLine($"RecursiveOdd({n}) = {RecursiveOdd(n)}");
				
		// När du har implementerat metoderna nedan kan du avkommentera raderna.

		Console.WriteLine($"RecursiveEven({n}) = {RecursiveEven(n)}");
		Console.WriteLine($"IterativeEven({n}) = {IterativeEven(n)}");
		Console.WriteLine($"FactorialRecursive({n}) = {FactorialRecursive(n)}");
		Console.WriteLine($"SumRecursive({n}) = {SumRecursive(n)}");
		Console.WriteLine($"SumIterative({n}) = {SumIterative(n)}");
		Console.WriteLine($"FibonacciRecursive({n}) = {FibonacciRecursive(n)}");
		Console.WriteLine($"FibonacciIterative({n}) = {FibonacciIterative(n)}");

		Console.WriteLine();
		Console.WriteLine("Trace av rekursion:");
		RecursiveOddWithTrace(n);

		Console.WriteLine();
		Console.WriteLine("Trace med indrag (visar rekursionsdjup):");
		RecursiveOddWithDepth(n, 0);

		// Vad är ett basfall?
		Console.WriteLine("Basfallet är det stoppvillkor som talar om för funktionen när den ska sluta anropa sig själv och istället börja returnera ett svar.");

		// Varför måste en rekursiv metod ha ett basfall?
		Console.WriteLine("Utan ett basfall skulle en rekursiv metod anropa sig själv i en oändlig loop.");

		// Vad händer på stacken när en metod anropar sig själv?
		Console.WriteLine("Varje gång metoden anropas skapas ett nytt minnesutrymme och läggs högst upp på stacken." +
			" Varje gång metoden anropas så ökar antalet \"lådor\" på stacken.");

		// Vilken version är mest minnesvänlig: rekursion eller iteration? Varför?
		Console.WriteLine("Iteration är mest minnesvänlig då den skapar upp lokala variabler i minnet som återanvänds. " +
			"Rekursion skapar om oändligt många gånger i minnet tills minnet är slut eller att basfallet stoppar loopen.");
	}
	static int RecursiveOdd(int n)
	{
		if (n <= 0)
		{
			throw new ArgumentException("talet måste vara större än 0.");
		}

		if (n == 1)
		{
			return 1;
		}
		return RecursiveOdd(n - 1) + 2;
	}
	static int RecursiveEven(int n)
	{
		// Om n <= 0, kasta ArgumentException med meddelandet "n måste vara större än 0."
		// Om n == 1, returnera 2.
		// Annars returnera RecursiveEven(n - 1) + 2.
		//
		// Exempel:
		// RecursiveEven(1) = 2
		// RecursiveEven(2) = 4
		// RecursiveEven(3) = 6

		if (n <= 0)
		{
			throw new ArgumentException("n måste vara större än 0.");
		}
		if (n == 1)
		{
			return 2;
		}
		return RecursiveEven(n - 1) + 2;
	}
	static int IterativeEven(int n)
	{
		// Implementera IterativeEven så att den returnerar det n:te jämna talet.
		// Om n <= 0, kasta ArgumentException.
		// Använd en for-loop för att räkna fram det n:te jämna talet.
		//
		// Exempel:
		// IterativeEven(1) = 2
		// IterativeEven(2) = 4
		// IterativeEven(3) = 6

		if (n <= 0)
			throw new ArgumentException("talet måste vara större än 0.");

		int counter = 0;
		for (int i = 1; i <= n; i++)
			counter += 2;

		return counter;
	}
	static int FactorialRecursive(int n)
	{
		// Fakultet:
		// 5! = 5 * 4 * 3 * 2 * 1 = 120
		//
		// Om n < 0, kasta ArgumentException.
		// Om n == 0 eller n == 1, returnera 1.
		// Annars returnera n * FactorialRecursive(n - 1).

		if (n < 0)
			throw new ArgumentException("talet måste vara större än eller lika med 0.");
		if (n == 0 || n == 1)
			return 1;
		return n * FactorialRecursive(n - 1);
	}
	static int SumRecursive(int n)
	{
		// Summera alla tal från 1 till n med rekursion.
		//
		// SumRecursive(5)
		// = 5 + 4 + 3 + 2 + 1
		// = 15

		if (n <= 0)
			throw new ArgumentException("talet måste vara större än 0.");
		if (n == 1)
			return 1;

		return n + SumRecursive(n - 1);
	}
	static int SumIterative(int n)
	{
		// Summera alla tal från 1 till n med en loop.
		int sum = 0;
		for (int i = 1; i <= n; i++)
		{
			sum += i;
		}
		return sum;
	}
	static int FibonacciRecursive(int n)
	{
		// Fibonacci:
		// 0, 1, 1, 2, 3, 5, 8, 13 ...
		//
		// Om n < 0, kasta ArgumentException.
		// Om n == 0, returnera 0.
		// Om n == 1, returnera 1.
		// Annars returnera FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2).

		if (n < 0)
			throw new ArgumentException("talet måste vara större än eller lika med 0.");
		if (n == 0) return 0;
		if (n == 1) return 1;

		return FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2);
	}
	static int FibonacciIterative(int n)
	{
		// Implementera Fibonacci med loop.
		// Denna version ska vara mer minnesvänlig än den rekursiva.

		if (n < 0)
			throw new ArgumentException("talet måste vara större än eller lika med 0.");
		if (n == 0) return 0;
		if (n == 1) return 1;

		int firstValue = 0;
		int secondValue = 1;
		int result = 0;

		for (int i = 2; i <= n; i++)
		{
			result = firstValue + secondValue;
			firstValue = secondValue;
			secondValue = result;
		}
		return result;
	}
	static int RecursiveOddWithTrace(int n)
	{
		Console.WriteLine($"Anropar RecursiveOddWithTrace({n})");
		logMessages.Add($"Anropar RecursiveOddWithTrace({n})");

		if (n == 1)
		{
			Console.WriteLine("Basfall nått. Returnerar 1.");
			logMessages.Add("Basfall nått. Returnerar 1.");
			return 1;
		}

		int result = RecursiveOddWithTrace(n - 1) + 2;

		Console.WriteLine($"RecursiveOddWithTrace({n}) returnerar {result}");
		logMessages.Add($"RecursiveOddWithTrace({n}) returnerar {result}");			

		return result;
	}
	static int RecursiveOddWithDepth(int n, int depth)
	{
		string indentation = new string(' ', depth * 2);

		Console.WriteLine($"{indentation}RecursiveOddWithDepth({n})");

		// Lägg till basfall: om n == 1, skriv ut med indrag att basfallet nåtts och returnera 1.
		// Annars: anropa RecursiveOddWithDepth(n - 1, depth + 1) rekursivt.
		// Spara resultatet, skriv ut med indrag vad metoden returnerar, och returnera resultatet.
		// Jämför utskriften med RecursiveOddWithTrace — vad tillför indraget?

		// Indragen visar varje gång metoden anropas så ökar stacken och varje gång den returnerar så minskar stacken.
		// Det gör det lättare att se hur många gånger metoden anropas och när den börjar returnera.
		// Det visar också tydligt när basfallet nås och när metoden börjar returnera värden.			

		if (n == 1)
		{
			Console.WriteLine($"{indentation}Basfall nått. Returnerar 1.");			
			return 1;
		}
		else
		{
			int result = RecursiveOddWithDepth(n - 1, depth + 1) + 2;
			Console.WriteLine($"{indentation}RecursiveOddWithDepth({n}) returnerar {result}");			
			return result;
		}
	}

	// ============================================================
	// DEL 10 - FILHANTERING, EXTRA
	// ============================================================

	static void SaveLogToFile()
	{		
		// Kontrollera om logMessages är tom — skriv meddelande om den är det.
		// Annars: spara alla loggmeddelanden till en fil som heter "logg.txt".
		// Skriv ut hur många rader som sparades och var filen finns.
		//
		// Tips:
		// File.WriteAllLines("logg.txt", logMessages);
		Console.WriteLine($"Sparade {logMessages.Count} rader till logg.txt");

		if (logMessages.Count == 0 || logMessages == null)
			Console.WriteLine("Inga loggmeddelanden att spara.");
		else
		{
			try
			{
				string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\logg.txt");

				//IF FILE DOES NOT EXIST, CREATE FILE
				if (!File.Exists(path))
					File.WriteAllLines(path, logMessages, Encoding.UTF8);
				else
				{
					//OVERWRITE FILE IF EXISTS
					File.WriteAllLines(path, logMessages, Encoding.UTF8);
				}				
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}
	#endregion
}