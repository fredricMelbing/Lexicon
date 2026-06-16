using VehicleHub.ConsoleApp.Models;
using VehicleHub.ConsoleApp.Storage;

namespace VehicleHub.Tests
{
	public class GarageTests
	{
		[Fact]
		public void Park_WhenSpaceIsAvailable_ShouldPlaceVehicleAndReturnTrue()
		{
			// ARRANGE			
			var garage = new Garage<Car>(2);
			var car = new Car("XYZ987", "Silver", 4, "El");

			// ACT
			bool result = garage.Park(car);

			// ASSERT
			Assert.True(result);					
			Assert.Contains(car, garage);
		}

		[Fact]
		public void Remove_WhenVehicleExists_ShouldClearSpaceAndReturnTrue()
		{
			// ARRANGE
			var garage = new Garage<Car>(3);
			var car = new Car("TUV456", "Grön", 4, "Gas");
			garage.Park(car);

			// ACT
			bool removeResult = garage.Remove("TUV456");

			// ASSERT
			Assert.True(removeResult);
			Assert.Empty(garage);
		}

		[Fact]
		public void Remove_WithNonExistingRegNum_ShouldReturnFalse()
		{
			// ARRANGE
			var garage = new Garage<Car>(2);
			var car = new Car("AAA111", "Svart", 4, "Bensin");
			garage.Park(car);

			// ACT			
			bool removeResult = garage.Remove("BBB222");

			// ASSERT
			Assert.False(removeResult);						
			Assert.Single(garage);
		}

		[Fact]
		public void GetEnumerator_ShouldOnlyReturnNonNullVehicles()
		{
			// ARRANGE
			var garage = new Garage<Car>(5);
			var car1 = new Car("CCC111", "Vit", 4, "Diesel");
			var car2 = new Car("DDD222", "Blå", 4, "El");

			garage.Park(car1);
			garage.Park(car2);

			// ACT			
			// With .Count() we force a foreach loop via GetEnumerator()
			int count = garage.Count();

			// ASSERT			
			// Test should show 2. If yield return had not filtered out the null spaces			
			Assert.Equal(2, count);
		}
	}
}
