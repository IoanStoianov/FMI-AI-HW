using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialInteligenceHW2
{
	public class Queens
	{
		private readonly int N;

		public int[] queens;

		public int[] d1;

		public int[] d2;

		public int[] rows;

		private int b;


		private Random rand = new Random();
		public Queens(int n)
		{
			this.N = n;
			queens = new int[N];
			d1 = new int[2 * N - 1];
			d2 = new int[2 * N - 1];
			rows = new int[N];
			b = N - 1;
			this.Init();
		}

		private void CalculateConflicts()
		{
			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] == i)
					{
						conflicts++;
					}
					if (rows[i] < 0) rows[i] = 0;
					rows[i] = conflicts;
				}
			}

			for (int i = 0; i < 2 * N - 1; i++)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] + j == i)
					{
						conflicts++;
					}
				}
				if (conflicts < 0) conflicts = 0;
				d1[i] = conflicts;
			}

			for (int i = N - 1; -N < i; i--)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] - j == i)
					{
						conflicts++;
					}
				}
				if (conflicts < 0) conflicts = 0;
				d2[b+i] = conflicts;
			}
		}

		public int GetColWithMaxConflicts()
		{
			var arr = new List<int>();
			int maxConflicts = 1;
			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;
				int row = queens[i];

				conflicts += rows[row];

				conflicts += d1[row + i];
				conflicts += d2[b+row - i];


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

			if (arr.Count == 0) return -1;
			int index = rand.Next(arr.Count);
			return arr[index];
		}

		public int GetRowWithMinConflicts(int col)
		{
			var arr = new List<int>();
			int minConflicts = N;
			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;

				conflicts += rows[i];

				conflicts += d1[i + col];
				conflicts += d2[b+i - col];

				if (conflicts == minConflicts)
				{
					arr.Add(i);
				}

				if (conflicts < minConflicts)
				{
					minConflicts = conflicts;
					arr = new List<int>()
					{
						i
					};
				}
			}

			if (arr.Count == 0) return -1;
			int index = rand.Next(arr.Count);
			return arr[index];
		}

		public void MooveQueen(int col, int row)
		{
			rows[queens[col]]--;
			d1[queens[col]+col]--;
			d2[b+queens[col]-col]--;
			
			queens[col] = row;

			rows[queens[col]]++;
			d1[queens[col] + col]++;
			d2[b+queens[col] - col]++;
		}

		public void Init()
		{
			
			if (N % 2 == 0)
			{
				InitEven(N);
			}
			else
			{
				InitEven(N - 1);
			}
			this.queens[N-1] = N-1;
			this.CalculateConflicts();
		}

		private void InitEven(int N)
		{
			int k = 0;
			for (int i = 1; i < N; i += 2)
			{
				this.queens[i] = k;
				k++;
			}
			k = N / 2;
			for (int i = 0; i < N; i += 2)
			{
				this.queens[i] = k;
				k++;
			}
		}

		public void PrintQueens()
		{
			for (int i = 0; i < N; i++)
			{
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] == i) Console.Write("* ");

					else Console.Write("_ ");
				}
				Console.WriteLine();
			}

			Console.WriteLine();
		}

		public bool IsValid()
		{

			for (int i = 0; i < N; i++)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] == i)
					{
						conflicts++;
					}
				}
				if (conflicts > 1) return false;
			}

			for (int i = 0; i < 2 * N - 1; i++)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] + j == i)
					{
						conflicts++;
					}
				}
				if (conflicts > 1) return false;
			}

			for (int i = N - 1; -N < i; i--)
			{
				int conflicts = 0;
				for (int j = 0; j < N; j++)
				{
					if (this.queens[j] - j == i)
					{
						conflicts++;
					}
				}
				if (conflicts > 1) return false;
			}
			return true;
		}
	}
}
