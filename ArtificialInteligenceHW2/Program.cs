using System;
using System.Collections.Generic;

namespace ArtificialInteligenceHW2
{
	class Program
	{
		public static int K = 5;
		static void Main(string[] args)
		{
			int N = 10001;
			var queens = new Queens(N);
			var watch = new System.Diagnostics.Stopwatch();
			
			watch.Start();
			while (!Search(queens, N));
			watch.Stop();

			Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

			if (N <= 50) queens.PrintQueens();
			Console.WriteLine("Done");
		}

		private static bool Search(Queens queens, int N)
		{
			int col, row;
			for (int i = 0; i < N * K; i++)
			{
				if (queens.IsValid())
				{
					return true;
				}
				col = queens.GetColWithMaxConflicts();
				if (col == -1) break;

				row = queens.GetRowWithMinConflicts(col);
				if (row == -1) break;

				queens.MooveQueen(col, row);
			}
			return queens.IsValid();
		}

		private static bool UnoptimizedSearch(Queens queens, int N)
		{
			queens.Init();
			int col, row;
			for (int i = 0; i < N * K; i++)
			{
				if (N <= 50) queens.PrintQueens();

				if (queens.IsValid())
				{
					return true;
				}
				col = GetColWithMaxConflicts(queens.queens);
				if (col == -1) break;

				row = GetRowWithMinConflicts(queens.queens, col);
				if (row == -1) break;

				queens.MooveQueen(col, row);
			}
			return queens.IsValid();
		}

		private static int GetRowWithMinConflicts(int[] queens, int col)
		{
			var rand = new Random();
			int N = queens.Length;
			int minCount = N+1;

			var arr = new List<int>();

			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (queens[j] == i)
					{
						conflicts++;
					}
				}
				
				int d1 = i + col;
				int d2 = i - col;

				for (int j = 0; j < N; j++)
				{
					if (queens[j] + j == d1)
					{
						conflicts++;
					}

					if(queens[j] - j == d2)
					{
						conflicts++;
					}
				}
				
				if (conflicts == minCount)
				{
					arr.Add(i);
				}

				if(conflicts < minCount)
				{
					minCount = conflicts;
					arr = new List<int>()
					{
						i
					};
				}
			}
			int index = rand.Next(arr.Count);
			return arr[index];
		}

		private static int GetColWithMaxConflicts(int[] queens)
		{
			var rand = new Random();

			int N = queens.Length;

			var arr = new List<int>() { };
			int maxConflicts = 0;

			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;

				for (int j = 0; j < N; j++)
				{
					if (queens[j] == queens[i] && j != i)
					{
						conflicts++;
					}
				}

				int d1 = queens[i] + i;
				int d2 = queens[i] - i;

				for (int j = 0; j < N; j++)
				{
					if(j != i && queens[j] + j == d1)
					{
						conflicts++;
					}

					if (j != i && queens[j] - j == d2)
					{
						conflicts++;
					}
				}

				if (conflicts == maxConflicts)
				{
					arr.Add(i);
				}

				if (conflicts > maxConflicts)
				{
					maxConflicts = conflicts;
					arr = new List<int>()
					{
						i
					};
				}


			}
			int index = rand.Next(arr.Count);
			return arr[index];
		}

		
	}
}
