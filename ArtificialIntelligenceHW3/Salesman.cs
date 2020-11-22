using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtificialInteligenceHW3
{
	public class Salesman
	{
		private int populationNum = 100;

		private double[,] pointDistance;

		private int[][] parentPopulation;

		private double[] parentEvaluation;

		private int[][] childPopulation;

		private HashSet<string> populationCheck;

		private int childIndex;

		private Random rand;

		public Salesman(int n)
		{
			this.parentPopulation = new int[(int)(this.populationNum*1.5)][];
			this.childPopulation = new int[this.populationNum/2][];
			this.pointDistance = new double[n, n];
			this.rand = new Random();
			GenerateCitiesDistances(n);
		}

		public void GenerateCitiesDistances(int n)
		{
			
			var temp = new Point[n];
			for (int i = 0; i < n; i++)
			{
				temp[i] = new Point(this.rand.Next(10000), this.rand.Next(10000));
			}

			for (int i = 0; i < n; i++)
			{
				for (int j = i; j < n; j++)
				{
					double pitagorDistance = this.CalculatePitagorDistance(temp[i], temp[j]);
					this.pointDistance[i, j] = pitagorDistance;
					this.pointDistance[j, i] = pitagorDistance;
				}
			}
		}

		private double CalculatePitagorDistance(Point p1, Point p2)
		{
			return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}

		private double CalculateSalesmanPath(int [] path)
		{
			double sum = 0;
			for (int i = 0; i < path.Length-1; i++)
			{
				sum += pointDistance[path[i], path[i + 1]];
			}
			sum += pointDistance[0, path.Length - 1];
			return sum;
		}

		private void GenerateInitialPopulation()
		{
			for (int i = 0; i < this.populationNum; i++)
			{
				this.parentPopulation[i] = Enumerable.Range(0, 100).OrderBy(c => rand.Next()).ToArray();
			}

			parentEvaluation = new double[(int)(this.populationNum * 1.5)];
			for (int i = 0; i < populationNum; i++)
			{
				parentEvaluation[i] = CalculateSalesmanPath(parentPopulation[i]);
			}

			var fittest = new SortedDictionary<double, int[]>();
			this.populationCheck = new HashSet<string>();

			string check;

			for (int i = 0; i < populationNum; i++)
			{
				fittest.Add(parentEvaluation[i], parentPopulation[i]);
				check = string.Join("", this.parentPopulation[i]);
				this.populationCheck.Add(check);
			}

			int k = 0;
			foreach (var elem in fittest)
			{
				parentPopulation[k] = elem.Value;
				parentEvaluation[k] = elem.Key;
				k++;
			}
		}

		public void GeneticEvolution()
		{
			GenerateInitialPopulation();

			for (int i = 1; i <= 8000; i++)
			{
				this.Reproduce();
				this.Mutate();
				this.Evaluate();
				if (i % 2000 == 0 || i == 10)
				{
					Console.WriteLine("Iteration {0}: {1}", i, Math.Floor(parentEvaluation[0]));
				}
			}
			
		}

		private void Evaluate()
		{
			var childEvaluation = new double[this.populationNum / 2];
			for (int i = 0; i < this.populationNum / 2; i++)
			{
				childEvaluation[i] = CalculateSalesmanPath(this.childPopulation[i]);
			}

			var fittest = new SortedDictionary<double, int[]>();
			this.populationCheck = new HashSet<string>();

			string check;

			for (int i = 0; i < this.populationNum; i++)
			{
				check = string.Join("", this.parentPopulation[i]);
				fittest.Add(this.parentEvaluation[i], this.parentPopulation[i]);
				this.populationCheck.Add(check);
			}
			for (int i = 0; i < this.populationNum / 2; i++)
			{
				if (fittest.ContainsKey(childEvaluation[i]))
					continue;

				check = string.Join("", this.childPopulation[i]);
				fittest.Add(childEvaluation[i], this.childPopulation[i]);
				this.populationCheck.Add(check);
			}

			// gets the best of parent and child population
			int j = 0;
			foreach(var elem in fittest)
			{
				this.parentPopulation[j] = elem.Value;
				this.parentEvaluation[j] = elem.Key;
				j++;
			} 
		}

		private void Mutate()
		{
			while (this.childIndex < this.populationNum/2)
			{
				int n = rand.Next(0, populationNum);

				int[] save = (int[])parentPopulation[n].Clone();
				switch (n % 3)
				{
					case 0: this.SwapMutation(save); break;
					case 1: this.InsertMutation(save); break;
					case 2: this.ReverseMutation(save); break;
				}

				string check = string.Join("", save);
				if (!this.populationCheck.Contains(check))
				{
					this.childPopulation[childIndex] = save;
					this.populationCheck.Add(check);
					this.childIndex++;
				}
			}
			
		}

		private void Reproduce()
		{
			for (this.childIndex = 0; childIndex < 0.7 * (this.populationNum / 2);)
			{
				Tuple<int, int> indexes = this.GetDifferendRandomIndexes(this.populationNum / 2, true);
				int n = indexes.Item1;
				int m = indexes.Item2;

				int[] save1 = (int[])parentPopulation[n].Clone();
				int[] save2 = (int[])parentPopulation[m].Clone();

				switch (n % 2)
				{
					case 0:
						{
							this.OnePointCrossover(save1, save2);
							break;
						}
					case 1:
						{
							this.TwoPointsCrossover(save1, save2);
							break;
						}
				}
				string check = string.Join("", save1);
				if (!this.populationCheck.Contains(check))
				{
					this.childPopulation[childIndex] = save1;
					this.populationCheck.Add(check);
					this.childIndex++;
				}

				check = string.Join("", save2);
				if (!this.populationCheck.Contains(check))
				{
					this.childPopulation[childIndex] = save2;
					this.populationCheck.Add(check);
					this.childIndex++;
				}
			}
		}

		public void CycleCrossover(int[] path1, int[] path2)
		{
			var indexMap1 = new Dictionary<int, int>();
			var indexMap2 = new Dictionary<int, int>();

			var p2p1 = new Dictionary<int, int>();
			var p2p2 = new Dictionary<int, int>();

			for (int i = 0; i < path1.Length; i++)
			{
				indexMap1.Add(i, path1[i]);
				indexMap2.Add(i, path2[i]);
				p2p1.Add(path1[i], path2[i]);
				p2p2.Add(path2[i], path1[i]);
			}

			var used = new HashSet<int>();
			int j = 0;
			while(used.Count < 100)
			{
				if (!used.Contains(path1[j]))
				{

				}
			}
		}

		public void OnePointCrossover(int[] path1, int[] path2)
		{
			int index = rand.Next(0, path1.Length);
			int[] save = (int[])path1.Clone();
			OnePointCrossoverHelper(path1, path2, index);
			OnePointCrossoverHelper(path2, save, index);
		}

		private void OnePointCrossoverHelper(int[] path1, int[] path2, int index)
		{
			var used = new HashSet<int>();
			int i;
			for (i = 0; i < index; i++)
			{
				used.Add(path1[i]);
			}
			for (int j = 0; j < path1.Length; j++)
			{
				if (!used.Contains(path2[j]))
				{
					path1[i] = path2[j];
					used.Add(path2[j]);
					i++;
				}
			}
		}

		public void TwoPointsCrossover(int[] path1, int[] path2)
		{
			Tuple<int, int> indexes = this.GetDifferendRandomIndexes(path1.Length, true);
			int index1 = indexes.Item1;
			int index2 = indexes.Item2;

			int[] save = (int[])path1.Clone();
			TwoPointsCrossoverHelper(path1, path2, index1, index2);
			TwoPointsCrossoverHelper(path2, save, index1, index2);
		}

		private void TwoPointsCrossoverHelper(int[] path1, int[] path2, int index1, int index2)
		{
			var used = new HashSet<int>();
			int i;
			for (i = index1; i <= index2; i++)
			{
				used.Add(path1[i]);
			}

			for (int j = i; j < path1.Length; j++)
			{
				if (!used.Contains(path2[j]))
				{
					path1[i] = path2[j];
					used.Add(path2[j]);
					i++;
					if (i == path1.Length) i = 0;
				}
			}
			if (i == path1.Length) i = 0;
			for (int j = 0; j < path1.Length; j++)
			{
				if (!used.Contains(path2[j]))
				{
					path1[i] = path2[j];
					used.Add(path2[j]);
					i++;
					if (i == path1.Length) i = 0;
				}
			}
		}

		public void SwapMutation(int[] path)
		{
			Tuple<int, int> indexes = this.GetDifferendRandomIndexes(path.Length);
			int index1 = indexes.Item1;
			int index2 = indexes.Item2;

			int save = path[index1];
			path[index1] = path[index2];
			path[index2] = save;
		}

		public void InsertMutation(int[] path)
		{
			Tuple<int, int> indexes = this.GetDifferendRandomIndexes(path.Length);
			int index1 = indexes.Item1;
			int index2 = indexes.Item2;

			int save = path[index1];
			for (int i = index1; i < index2; i++)
			{
				path[i] = path[i + 1];
			}
			path[index2] = save;
		}

		public void ReverseMutation(int[] path)
		{
			Tuple<int, int> indexes = this.GetDifferendRandomIndexes(path.Length, true);
			int index1 = indexes.Item1;
			int index2 = indexes.Item2;
			while (index1 < index2)
			{
				int temp = path[index1];
				path[index1] = path[index2];
				path[index2] = temp;
				index1++;
				index2--;
			}
		}

		private Tuple<int, int> GetDifferendRandomIndexes(int len, bool edgeIndexes = false)
		{
			int index1 = rand.Next(0, len);
			int index2 = rand.Next(index1, len);
			while (index1 == index2 || (edgeIndexes && index1 == 0 && index2 == len))
			{
				index1 = rand.Next(0, len);
				index2 = rand.Next(index1, len);
			}
			return new Tuple<int, int>(index1, index2);
		}

		public struct Point
		{
			public readonly int X;
			public readonly int Y;

			public Point(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}
	}
}
