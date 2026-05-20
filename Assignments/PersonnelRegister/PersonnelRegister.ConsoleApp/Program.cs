using PersonnelRegister.Core.Interface;
using PersonnelRegister.Core.Service;

namespace PersonnelRegister.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IMenyService _menyService = new MenyService();
			_menyService.Run();

		}
	}
}
