using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonnelRegister.Domain.Entities
{
	public class Personnel
	{
		[StringLength(100)]
		public string FirstName { get; set; }
		[StringLength(100)]
		public string LastName { get; set; }
		[Required]
		public decimal Salary { get; set; } = 0;

		public Personnel(string firstName, string lastName, decimal salary)
		{
			string FirstName = firstName;
			string LastName = lastName;
			decimal Salary = salary;
		}
		public Personnel()
		{
			
		}
	}
}
