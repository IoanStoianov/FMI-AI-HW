using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ArtificialIntelligenceHW7
{
	class Program
	{
		static void Main(string[] args)
		{
			string line;
			var arr = new List<Point>();

			Console.WriteLine("Enter number of clusters");
			int K = Int32.Parse(Console.ReadLine());

			Console.WriteLine("Enter 1 fo normal and 2 for unbalanced");
			int n = Int32.Parse(Console.ReadLine());

			bool check = n == 1;

			StreamReader file;
			if (check)
			{
				file = new StreamReader(@"C:\Users\ioan6\OneDrive\Desktop\II\normal\normal.txt");
			}
			else
			{
				file = new StreamReader(@"C:\Users\ioan6\OneDrive\Desktop\II\unbalance\unbalance.txt");
			}

			while ((line = file.ReadLine()) != null)
			{
				string[] str = line.Split();
				var point = new Point(Convert.ToDouble(str[0]), Convert.ToDouble(str[1]));
				arr.Add(point);
			}
			file.Close();

			KMeans kmeans;
			if (check)
			{
				kmeans = new KMeans(arr.ToArray(), K, 6);
			}
			else
			{
				kmeans = new KMeans(arr.ToArray(), K, 500000);
			}
			kmeans.RandonRestart();
			Console.WriteLine("Done!");
		}
	}
}
