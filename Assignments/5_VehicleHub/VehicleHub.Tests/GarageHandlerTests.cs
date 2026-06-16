using VehicleHub.ConsoleApp.Interfaces;
using VehicleHub.ConsoleApp.Logic;
using VehicleHub.ConsoleApp.Models;

namespace VehicleHub.Tests
{
	public class GarageHandlerTests
	{
		[Fact]
		public void ParkVehicle_WithDuplicateRegistrationNumber_ShouldReturnFalse()
		{
			// ARRANGE			
			IGarageHandler handler = new GarageHandler();
			handler.CreateGarage(5);

			var firstCar = new Car("ABC123", "Blå", 4, "Bensin");
			var duplicateCar = new Car("abc123", "Röd", 4, "Diesel");
						
			bool firstParkResult = handler.ParkVehicle(firstCar);

			// ACT
			bool duplicateParkResult = handler.ParkVehicle(duplicateCar);

			// ASSERT
			Assert.True(firstParkResult);
			Assert.False(duplicateParkResult);
		}

		[Fact]
		public void ParkVehicle_WhenGarageIsFull_ShouldReturnFalse()
		{
			// ARRANGE
			IGarageHandler handler = new GarageHandler();
			handler.CreateGarage(1);

			var car1 = new Car("AAA111", "Svart", 4, "El");
			var car2 = new Car("BBB222", "Vit", 4, "Diesel");

			handler.ParkVehicle(car1);

			// ACT
			bool result = handler.ParkVehicle(car2);

			// ASSERT
			Assert.False(result);
		}
	}
}
