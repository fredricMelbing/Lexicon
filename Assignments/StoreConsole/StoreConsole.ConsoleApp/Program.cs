using StoreConsole.DemoClasses;
using StoreConsole.Helpers;
using StoreConsole.StackAndHeap;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;

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
        while(running);
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
		products.Select(pd => pd.Value).ToList()
            .ForEach(p => Console.WriteLine($"{p}: Total lagervärde: {p.Price * p.Stock}"));
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
        if(products.TryGetValue(InputHelpers.ReadString("Ange produktkod: ").ToUpper(), out Product? product))
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
        if(customerQueue.Count > 0)
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
				Console.WriteLine($"{customerQueue.ToList().IndexOf(c)+1}. {c.Name} ({c.AddedAt})");
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
            else if(product is null)
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
                product.Stock ++;
                logMessages.Add("Ångrat köp: "+sale.ToString());                
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

		// TODO: 13. Skriv ut loggmeddelanden.
		// Om logMessages är tom, skriv "Inga loggmeddelanden finns."
		// Annars: loopa igenom logMessages och skriv ut varje meddelande.

		Console.WriteLine("TODO: Implementera PrintLog.");

		// TODO Fråga8: Varför passar List bra för loggmeddelanden?
		// Varför passar List bra för loggmeddelanden?
		Console.WriteLine("Svar: TODO - skriv ditt svar här");
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

		// TODO: 14. Lägg till minst 4 egna varor med en loop.
		// Lägg till minst 4 egna varor med en loop.
		// Skriv ut hela listan.

		// TODO: Fråga9.1: Vad betyder Count?
		// Vad betyder Count?
		Console.WriteLine("Svar 1: TODO - skriv ditt svar här");

		// TODO: Fråga9.2: Vad betyder Capacity?
		// Vad betyder Capacity?
		Console.WriteLine("Svar 2: TODO - skriv ditt svar här");

		// TODO: Fråga9.3: Varför ökar inte Capacity med exakt 1 varje gång?
		// Varför ökar inte Capacity med exakt 1 varje gång?
		Console.WriteLine("Svar 3: TODO - skriv ditt svar här");

		// TODO: Fråga9.4: Minskar Capacity automatiskt när element tas bort?
		// Minskar Capacity automatiskt när element tas bort?
		Console.WriteLine("Svar 4: TODO - skriv ditt svar här");
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

		// TODO: 15. Skriv ut alla veckodagar med en for-loop.
		// Skriv ut alla veckodagar med en for-loop.
		// Tips: använd weekdays.Length för att veta hur många element det finns.

		// TODO: 16. Skriv ut alla veckodagar med en foreach-loop.
		// Skriv ut alla veckodagar med foreach.

		Console.WriteLine("TODO: Implementera utskrifter i ArrayLab.");

		// TODO: Fråga10.1: När passar en array bättre än en List?
		// När passar en array bättre än en List?
		Console.WriteLine("Svar 1: TODO - skriv ditt svar här");

		// TODO: Fråga10.2: Vad händer om du försöker skriva weekdays[5]?
		// Vad händer om du försöker skriva weekdays[5]?
		Console.WriteLine("Svar 2: TODO - skriv ditt svar här");

		// TODO: Fråga10.3: Varför måste arrayens storlek anges från början?
		// Varför måste arrayens storlek anges från början?
		Console.WriteLine("Svar 3: TODO - skriv ditt svar här");
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
        string text = ReadLine;

        //ToDo: 17. Skriv koden för CountWords
        Dictionary<string, int> wordCounts = CountWords(text);

        Console.WriteLine("Resultat:");

        foreach (KeyValuePair<string, int> pair in wordCounts)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

		// TODO: Fråga11: Varför passar Dictionary bra när vi ska räkna ord?
		// Varför passar Dictionary bra när vi ska räkna ord?
		Console.WriteLine("Svar: TODO - skriv ditt svar här");
    }

    static Dictionary<string, int> CountWords(string text)
    {
        Dictionary<string, int> wordCounts = new Dictionary<string, int>();

		// TODO: 18. Implementera CountWords
		// Dela upp text i ord med string.Split.
		// Separera på: mellanslag (ett eller flera), punkt, !, ?, :, ;
		// Tips: string[] words = text.Split(new char[] { ' ', '.', '!', '?', ':', ';' },
		//                                   StringSplitOptions.RemoveEmptyEntries);
		//
		// Loopa igenom orden.
		// Gör varje ord till gemener med .ToLower() så att "Hej" och "hej" räknas som samma.
		// Om ordet redan finns i wordCounts → öka värdet med 1.
		// Annars → lägg till ordet med värdet 1.

		// TODO: Fråga12: Vad är nyckeln och vad är värdet i wordCounts?
		// Vad är nyckeln och vad är värdet i wordCounts?
		Console.WriteLine("Svar: TODO - skriv ditt svar här");

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
        string input = ReadLine;

        //ToDo: 19. skriv koden för CheckParantheses
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
		// TODO: 20. Implementera CheckParentheses
		// Använd en Stack<char> och en Dictionary<char, char>.
		//
		// Tips Dictionary:
		// Låt dictionaryn mappa varje stängande parentes till sin matchande öppnare.
		// Det gör matchningskontrollen till en enkel uppslagning istället för flera if-satser.
		//
		// Tips Stack:
		// Stacken håller reda på vilka öppnare du sett men ännu inte stängt.
		// Tänk på vad LIFO innebär här — varför är det precis rätt egenskap för det här problemet?
		//
		// TODO: Fråga13: Varför är Dictionary + Stack bättre än bara Stack med if/else för matchningen?
		// Varför är Dictionary + Stack bättre än bara Stack med if/else för matchningen?
		Console.WriteLine("Svar: TODO - skriv ditt svar här");

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

		// TODO: Fråga13.1: Varför ändras inte number1 när number2 ändras?
		// Varför ändras inte number1 när number2 ändras?
		Console.WriteLine("Svar 1: TODO - skriv ditt svar här");

		// TODO: Fråga13.2: Varför ändras inte score1.Points när score2.Points ändras?
		// Varför ändras inte score1.Points när score2.Points ändras?
		Console.WriteLine("Svar 2: TODO - skriv ditt svar här");

		// TODO: Fråga13.3: Varför ändras product1.Stock när product2.Stock ändras?
		// Varför ändras product1.Stock när product2.Stock ändras?
		Console.WriteLine("Svar 3: TODO - skriv ditt svar här");

		// TODO: Fråga13.4: Är Product en value type eller reference type?
		// Är Product en value type eller reference type?
		Console.WriteLine("Svar 4: TODO - skriv ditt svar här");

		// TODO: Fråga13.5: Vad ligger på heapen i Product-exemplet?
		// Vad ligger på heapen i Product-exemplet?
		Console.WriteLine("Svar 5: TODO - skriv ditt svar här");

		// TODO: Fråga13.6: Vad innebär det att två variabler kan peka på samma objekt?
		// Vad innebär det att två variabler kan peka på samma objekt?
		Console.WriteLine("Svar 6: TODO - skriv ditt svar här");

		// TODO: Fråga13.7: Vad är skillnaden mellan stacken i minnet och Stack<T> som datastruktur?
		// Vad är skillnaden mellan stacken i minnet och Stack<T> som datastruktur?
		Console.WriteLine("Svar 7: TODO - skriv ditt svar här");
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

		// TODO: 21. Implementera RecursiveOdd så att den returnerar det n:te udda talet.
		// När du har implementerat metoderna nedan kan du avkommentera raderna.

		// Console.WriteLine($"RecursiveEven({n}) = {RecursiveEven(n)}");
		// Console.WriteLine($"IterativeEven({n}) = {IterativeEven(n)}");
		// Console.WriteLine($"FactorialRecursive({n}) = {FactorialRecursive(n)}");
		// Console.WriteLine($"SumRecursive({n}) = {SumRecursive(n)}");
		// Console.WriteLine($"SumIterative({n}) = {SumIterative(n)}");
		// Console.WriteLine($"FibonacciRecursive({n}) = {FibonacciRecursive(n)}");
		// Console.WriteLine($"FibonacciIterative({n}) = {FibonacciIterative(n)}");

		Console.WriteLine();
        Console.WriteLine("Trace av rekursion:");
        RecursiveOddWithTrace(n);

        Console.WriteLine();
        Console.WriteLine("Trace med indrag (visar rekursionsdjup):");
        RecursiveOddWithDepth(n, 0);

		// TODO: Fråga14.1: Vad är ett basfall?
		// Vad är ett basfall?
		Console.WriteLine("Svar 1: TODO - skriv ditt svar här");

		// TODO: Fråga14.2: Varför måste en rekursiv metod ha ett basfall?
		// Varför måste en rekursiv metod ha ett basfall?
		Console.WriteLine("Svar 2: TODO - skriv ditt svar här");

		// TODO: Fråga14.3: Vad händer på stacken när en metod anropar sig själv?
		// Vad händer på stacken när en metod anropar sig själv?
		Console.WriteLine("Svar 3: TODO - skriv ditt svar här");

		// TODO: Fråga14.4: Vilken version är mest minnesvänlig: rekursion eller iteration? Varför?
		// Vilken version är mest minnesvänlig: rekursion eller iteration? Varför?
		Console.WriteLine("Svar 4: TODO - skriv ditt svar här");
    }

    static int RecursiveOdd(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentException("n måste vara större än 0.");
        }

        if (n == 1)
        {
            return 1;
        }

        return RecursiveOdd(n - 1) + 2;
    }

    static int RecursiveEven(int n)
    {
		// TODO: 22. Implementera RecursiveEven så att den returnerar det n:te jämna talet.
		// Om n <= 0, kasta ArgumentException med meddelandet "n måste vara större än 0."
		// Om n == 1, returnera 2.
		// Annars returnera RecursiveEven(n - 1) + 2.
		//
		// Exempel:
		// RecursiveEven(1) = 2
		// RecursiveEven(2) = 4
		// RecursiveEven(3) = 6

		return 0;
    }

    static int IterativeEven(int n)
    {
		// TODO: 23. Implementera IterativeEven så att den returnerar det n:te jämna talet.
		// Om n <= 0, kasta ArgumentException.
		// Använd en for-loop för att räkna fram det n:te jämna talet.
		//
		// Exempel:
		// IterativeEven(1) = 2
		// IterativeEven(2) = 4
		// IterativeEven(3) = 6

		return 0;
    }

    static int FactorialRecursive(int n)
    {
		// TODO: 24. Implementera FactorialRecursive så att den returnerar n!.
		// Fakultet:
		// 5! = 5 * 4 * 3 * 2 * 1 = 120
		//
		// Om n < 0, kasta ArgumentException.
		// Om n == 0 eller n == 1, returnera 1.
		// Annars returnera n * FactorialRecursive(n - 1).

		return 0;
    }

    static int SumRecursive(int n)
    {
		// TODO: 25. Implementera SumRecursive så att den returnerar summan av de n första heltalen.
		// Summera alla tal från 1 till n med rekursion.
		//
		// SumRecursive(5)
		// = 5 + 4 + 3 + 2 + 1
		// = 15

		return 0;
    }

    static int SumIterative(int n)
    {
		// TODO: 26. Implementera SumIterative så att den returnerar summan av de n första heltalen.
		// Summera alla tal från 1 till n med en loop.

		return 0;
    }

    static int FibonacciRecursive(int n)
    {
		// TODO: 27. Implementera FibonacciRecursive så att den returnerar det n:te Fibonacci-talet.
		// Fibonacci:
		// 0, 1, 1, 2, 3, 5, 8, 13 ...
		//
		// Om n < 0, kasta ArgumentException.
		// Om n == 0, returnera 0.
		// Om n == 1, returnera 1.
		// Annars returnera FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2).

		return 0;
    }

    static int FibonacciIterative(int n)
    {
		// TODO: 28. Implementera FibonacciIterative så att den returnerar det n:te Fibonacci-talet.
		// Implementera Fibonacci med loop.
		// Denna version ska vara mer minnesvänlig än den rekursiva.

		return 0;
    }

    static int RecursiveOddWithTrace(int n)
    {
        Console.WriteLine($"Anropar RecursiveOddWithTrace({n})");

        if (n == 1)
        {
            Console.WriteLine("Basfall nått. Returnerar 1.");
            return 1;
        }

        int result = RecursiveOddWithTrace(n - 1) + 2;

        Console.WriteLine($"RecursiveOddWithTrace({n}) returnerar {result}");

        return result;
    }

    static int RecursiveOddWithDepth(int n, int depth)
    {
        string indentation = new string(' ', depth * 2);

        Console.WriteLine($"{indentation}RecursiveOddWithDepth({n})");

		// TODO: 29. Implementera RecursiveOddWithDepth så att den fungerar
		// Lägg till basfall: om n == 1, skriv ut med indrag att basfallet nåtts och returnera 1.
		// Annars: anropa RecursiveOddWithDepth(n - 1, depth + 1) rekursivt.
		// Spara resultatet, skriv ut med indrag vad metoden returnerar, och returnera resultatet.
		// Jämför utskriften med RecursiveOddWithTrace — vad tillför indraget?

		return 0;
    }

    // ============================================================
    // DEL 10 - FILHANTERING, EXTRA
    // ============================================================

    static void SaveLogToFile()
    {
		// TODO: 30. Spara loggutskriften från RecursiveOddWithTrace till en textfil.
		// Kontrollera om logMessages är tom — skriv meddelande om den är det.
		// Annars: spara alla loggmeddelanden till en fil som heter "logg.txt".
		// Skriv ut hur många rader som sparades och var filen finns.
		//
		// Tips:
		// File.WriteAllLines("logg.txt", logMessages);
		// Console.WriteLine($"Sparade {logMessages.Count} rader till logg.txt");

		Console.WriteLine("TODO: Implementera SaveLogToFile.");
    }

    #endregion
}
