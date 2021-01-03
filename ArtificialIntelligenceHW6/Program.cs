using System;
using System.Collections.Generic;

namespace ArtificialIntelligenceHW6
{
	//FN 81596
	//Йоан Стоянов
	class Program
	{
		static void Main(string[] args)
		{
			string line;
			var arr = new List<string[]>();

			// Read the file and display it line by line.  
			System.IO.StreamReader file =
				new System.IO.StreamReader(@"C:/Users/ioan6/Downloads/breast-cancer.data");
			while ((line = file.ReadLine()) != null)
			{
				arr.Add(line.Split(','));
			}
			file.Close();

			var model = new DecisionTree();
			model.TrainAndTest(arr.ToArray());
		}
	}
}
