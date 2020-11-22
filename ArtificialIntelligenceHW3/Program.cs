using System;

namespace ArtificialInteligenceHW3
{
	//FN 81596
	//Йоан Стоянов
	class Program
	{
		static void Main(string[] args)
		{
			var s = new Salesman(100);
			s.GeneticEvolution();
		}
	}
}
