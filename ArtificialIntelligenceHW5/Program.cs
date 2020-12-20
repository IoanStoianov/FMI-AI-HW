using System;

namespace ArtificialIntelligenceHW5
{
	//FN 81596
	//Йоан Стоянов
	class Program
	{
		static void Main(string[] args)
		{
			int counter = 0;
			string line;
			var arr = new string[435][];

			// Read the file and display it line by line.  
			System.IO.StreamReader file =
				new System.IO.StreamReader(@"C:/Users/ioan6/Downloads/house-votes-84.data");
			while ((line = file.ReadLine()) != null)
			{
				arr[counter] = line.Split(',');
				counter++;
			}
			file.Close();

			var model = new PoliticRecognizerModel(arr);
			model.TrainAndTest();
		}
	}
}
