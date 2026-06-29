using System.ComponentModel.DataAnnotations;

namespace LogisticsSystem.Web.Models
{
	public class Product
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; } = string.Empty;

		[Range(0, 1000000)]
		public int Price { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Order Date")]
		public DateTime Orderdate { get; set; }

		[Required]
		public string Category { get; set; } = string.Empty;

		[Required]
		public string Shelf { get; set; } = string.Empty;

		[Range(0, 10000)]
		public int Count { get; set; }

		public string Description { get; set; } = string.Empty;
	}
}
