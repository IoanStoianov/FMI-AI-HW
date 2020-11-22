using System;
using System.Collections.Generic;

namespace ArtificialIntelligenceHW1
{
	//FN 81596
	//Йоан Стоянов
	class Program
	{

		public static Tuple<int, int>[] ManhantanTable;

		static void Main(string[] args)
		{
			int number = Int32.Parse(Console.ReadLine());
			int size = (int)Math.Sqrt(number + 1);
			var start = new int[size, size];

			int zeroPosition = Int32.Parse(Console.ReadLine());
			int zeroX = 0, zeroY = 0;

			for (int i = 0; i < size; i++)
			{
				string input = Console.ReadLine();
				string[] arr = input.Split(' ');

				for (int j = 0; j < size; j++)
				{
					start[i, j] = Int32.Parse(arr[j]);
					if(start[i,j] == 0)
					{
						zeroX = j;
						zeroY = i;
					}
				}
			}

			var watch = new System.Diagnostics.Stopwatch();
			
			ManhantanTable = CalculateManhatanTable(size, zeroPosition);
			var board = new Board(start, new List<Directions>(), zeroX, zeroY);
			watch.Start();
			if (!board.IsSolvable() && zeroPosition == -1)
			{
				Console.WriteLine("Not Solvable");
				return;
			}
			List<Directions> result = IDAStar(board, ManhattanHeuristic);
			if (result == null)
			{
				Console.WriteLine("Not Solvable");
				return;
			}

			Console.WriteLine(result.Count);
			watch.Stop();
			foreach (var step in result)
			{
				Console.WriteLine(step);
			}

			Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

		}


		public static Tuple<int, int>[] CalculateManhatanTable(int size, int zeroPosition)
		{
			var table = new Tuple<int, int>[size*size+1];
			int x = 1;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (x == zeroPosition || (zeroPosition == -1 && j == size-1 && i == size-1))
					{
						table[0] = new Tuple<int, int>(i, j);
					}
					else
					{
						table[x] = new Tuple<int, int>(i, j);
					}
					x++;
				}
			}
			return table;
		}

		public static int ManhattanHeuristic(Board board)
		{
			return board.Path.Count + board.Manhattan;
		}

		public static List<Directions> IDAStar(Board board, Func<Board, int> heuristic)
		{
			if (board.IsGoal())
			{
				return new List<Directions>();
			}

			int i = heuristic(board);
			while (true)
			{
				int min = Int32.MaxValue;
				var stack = new Stack<Board>();
				stack.Push(board);

				var memo = new HashSet<string>();

				while (stack.Count != 0)
				{
					Board current = stack.Pop();

					foreach (var element in current.Neighbors())
					{
						if (element.IsGoal())
						{
							return element.Path;
						}
						if (heuristic(element) <= i)
						{
							if (!memo.Contains(element.BoardToString))
							{
								stack.Push(element);
								memo.Add(element.BoardToString);
							}
						}
						else
						{
							if (min > heuristic(element)) min = heuristic(element);
						}
					}
				}
				i = min;
			}
		
			return null;
		}

		public static List<Directions> AStar(Board board, Func<Board, int> heuristic)
		{
			if (board.IsGoal())
			{
				return new List<Directions>();
			}

			var queue = new C5.IntervalHeap<Board>(100, new BoardComparer(heuristic))
			{
				board
			};


			HashSet<string> memo = new HashSet<string>();
			while (!queue.IsEmpty)
			{
				Board current = queue.DeleteMin();
				foreach(var element in current.Neighbors())
				{
					if (element.IsGoal())
					{
						return element.Path;
					}
					if (!memo.Contains(element.BoardToString))
					{
						queue.Add(element);
						memo.Add(element.BoardToString);
					}
				}
			}
			return null;
		}
		
	}
}
