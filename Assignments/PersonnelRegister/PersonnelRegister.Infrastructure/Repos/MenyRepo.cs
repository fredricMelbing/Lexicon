using PersonnelRegister.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelRegister.Infrastructure.Repos
{
	public class MenyRepo : IMenyRepo
	{
		public List<string> GetMeny()
		{
			List<string> list = new List<string> {
				"Create new Emploeey",
				"Edit Employee",
				"Remove Employee",
				"Show Employee",
				"Exit"
			};

			return list;

			//return new List<string> {
			//	"Create new Emploeey",
			//	"Edit Employee",
			//	"Remove Employee",
			//	"Show Employee",
			//	"Exit"
			//};
		}
	}
}