namespace VehicleHub.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Manager manager = new Manager();
			manager.StartApplication();
		}
	}
}

//TODO: EXTRA Uppgift Unit testing (Att skriva test för hela applikationen ses som en extra uppgift om tid finns)
//Testa gärna med att skriva testen före ni implementerat funktionaliteten!
//Använd er sedan ctrl . för att generera era objekt och metoder. Flytta dessa genererade klasser till rätt projekt.
//Implementera sen funktionaliteten tills testet går igenom.


//TODO Extra: Förslag på Extra funktionalitet (ej krav):
//Möjlighet att också kunna söka på de fordonsspecifika egenskaperna.
//Hantera flera garage som kan ha olika typer av fordon i sig exempelvis en hangar ett vanligt garage samt ett motorcykelgarage.
//Detta kommer medföra att man ska kunna manövrera sig mellan dom olika garagen i ui:t
//för att kunna göra dom tidigare nämnda funktionerna de ska ske på bara det garaget som man har för tillfället valt.
//Det ska tydligt visas vilket garage man för närvarande arbetar med.
//Ett garage består inte längre av fordon utan av parkeringsplatser som i sin tur kan hålla fordon.
//Det går att via C# skriva och läsa till filsystemet från er applikation.
//Ta reda på hur man gör för att kunna spara ert garage (via meny eller automatiskt vid avstängning) och
//ladda in ert garage (via meny eller automatiskt vid start av applikationen)
//Olika fordon tar olika stor plats tex en bil tar 1plats en båt tar 2 platser
//ett flygplan kräver 3 platser osv en motorcykel tar endast 1/3 dels plats.
//När man parkerar ska endast de fordon som garaget har plats för visas som allternativ.
//Läsa in storleken på garaget via konfiguration.
//Valfri funktionalitet ni tycker borde finnas.