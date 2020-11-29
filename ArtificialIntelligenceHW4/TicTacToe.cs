using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligenceHW4
{
	class TicTacToe
	{
		public static int NotFinished = 2;
		public static int Player1 = 1;
		public static int Player2 = -1;

		private int[,] board;

		public TicTacToe()
		{
			this.board = new int[3, 3];
			//{ {1,1,-1 }, { 0, -1, 0 }, { 0, 0, 0 } };
		}

		public bool Add(int x, int y, bool computer)
		{
			if(board[y, x] != 0)
			{
				return false;
			}
			if (computer)
			{
				board[y, x] = Player1;
			}
			else
			{
				board[y, x] = Player2;
			}
			return true;
		}

		public int IsGameFinished(int [,] board = null)
		{
			if(board== null)
			{
				board = this.board;
			}
			bool draw = true;
			for (int i = 0; i < 3; i++)
			{
				if (board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2] && board[i, 0] != 0)
				{
					return board[i, 0];
				}
				if (board[0, i] == board[1, i] && board[0, i] == board[2, i] && board[0, i] != 0)
				{
					return board[0, i];
				}
				//draw check
				if(board[i, 0] == 0 || board[i, 1] == 0 || board[i, 2] == 0)
				{
					draw = false;
				}
			}
			if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2] && board[1, 1] != 0)
			{
				return board[0, 0];
			}
			if (board[2, 0] == board[1, 1] && board[2, 0] == board[0, 2] && board[1, 1] != 0)
			{
				return board[2, 0];
			}
			if (draw)
			{
				return 0;
			}
			return NotFinished;
		}

		public void PrintBoard()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					switch (board[i, j])
					{
						case 0: Console.Write("_ "); break;
						case 1: Console.Write("X "); break;
						case -1: Console.Write("O "); break;
					}
				}
				Console.WriteLine();
			}
		}

		private struct StepValue
		{
			public int Depth;
			public int Value;
			public int[,] Move;
			public StepValue(int depth, int value, int[,] move)
			{
				this.Depth = depth;
				this.Value = value;
				this.Move = move;
			}

			public static bool operator >=(StepValue a, StepValue b)
			{
				var temp = EvaluateMax2(b, a);
				return temp.GetHashCode() == b.GetHashCode();
			}
			public static bool operator <=(StepValue a, StepValue b)
			{
				var temp = EvaluateMax2(a, b);
				return temp.Value == b.Value && temp.Depth == b.Depth && temp.Move == b.Move;
			}
		}

		private static StepValue EvaluateMax(StepValue current, StepValue candidate, bool random = false)
		{
			if(current.Value < candidate.Value)
			{
				return candidate;
			}
			if (random && current.Value == candidate.Value)
			{
				return RandomMove(current, candidate);
			}

			return current;
		}

		private static StepValue EvaluateMin(StepValue current, StepValue candidate, bool random = false)
		{
			if (current.Value > candidate.Value)
			{
				return candidate;
			}
			if (random && current.Value == candidate.Value)
			{
				return RandomMove(current, candidate);
			}

			return current;
		}

		private static StepValue EvaluateMax2(StepValue current, StepValue candidate, bool random = false)
		{
			if (current.Value < candidate.Value)
			{
				return candidate;
			}
			if (current.Value == candidate.Value && current.Depth != candidate.Depth)
			{
				return CompareDepth(current, candidate);
			}

			if (random && current.Value == candidate.Value)
			{
				return RandomMove(current, candidate);
			}

			return current;
		}

		private StepValue EvaluateMin2(StepValue current, StepValue candidate, bool random = false)
		{
			if (current.Value > candidate.Value)
			{
				return candidate;
			}
			if (current.Value == candidate.Value && current.Depth != candidate.Depth)
			{
				return CompareDepth(current, candidate);
			}

			if (random && current.Value == candidate.Value)
			{
				return RandomMove(current, candidate);
			}

			return current;
		}

		private static StepValue CompareDepth(StepValue current, StepValue candidate)
		{
			if (current.Depth > candidate.Depth)
			{
				return candidate;
			}
			return current;
		}

		private static StepValue RandomMove(StepValue current, StepValue candidate)
		{
			Random rand = new Random();
			if (rand.NextDouble() >= 0.5)
			{
				return candidate;
			}
			else
			{
				return current;
			}
		}

		private StepValue MiniMax(int [,] currentMove, int depth, bool isComputer)
		{
			StepValue best, value;

			int finished = this.IsGameFinished(currentMove);
			if (finished != NotFinished)
			{
				return new StepValue(depth, finished, currentMove);
			}

			if (isComputer)
			{
				best = new StepValue(0, Int32.MinValue, board);

				foreach ( var move in GetMoves(Player1, currentMove))
				{
					value = MiniMax(move, depth + 1, false);

					best = EvaluateMax(best, new StepValue(depth, value.Value, move), true);
				}

				return best;

			}
			else
			{
				best = new StepValue(0, Int32.MaxValue, board);

				foreach (var move in GetMoves(Player2, currentMove))
				{
					value = MiniMax(move, depth + 1, true);

					best = EvaluateMin(best, new StepValue(depth, value.Value, move), true);
				}

				return best;
			}
		}

		private StepValue AlphaBetaSearch(int[,] currentMove, int depth, bool isComputer, StepValue alpha, StepValue beta)
		{
			StepValue best, result;

			int finished = this.IsGameFinished(currentMove);
			if (finished != NotFinished)
			{
				return new StepValue(depth, finished, currentMove);
			}

			if (isComputer)
			{
				best = new StepValue(0, Int32.MinValue, board);

				foreach (var move in GetMoves(Player1, currentMove))
				{
					result = AlphaBetaSearch(move, depth + 1, false, alpha, beta);
					best = EvaluateMax2(best, new StepValue(depth, result.Value, move));

					alpha = EvaluateMax2(alpha, best);
					if (beta <= alpha)
						break;
				}

				return best;

			}
			else
			{
				best = new StepValue(0, Int32.MaxValue, board);

				foreach (var move in GetMoves(Player2, currentMove))
				{
					result = AlphaBetaSearch(move, depth + 1, true, alpha, beta);
					best = EvaluateMin2(best, new StepValue(depth, result.Value, move));

					beta = EvaluateMin2(beta, best);
					if (beta <= alpha)
						break;
				}

				return best;
			}
		}

		private IEnumerable<int[,]> GetMoves(int player, int [,] board)
		{
			var moves = new List<int[,]>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if(board[i,j] == 0)
					{
						int[,] move = (int[,])board.Clone();
						move[i, j] = player;
						moves.Add(move);
					}
				}
			}
			return moves;
		}

		public void Game()
		{
			bool firstStep = true;
			bool isComputer = true;
			int first = 0;


			Console.WriteLine("Who is first?");
			Console.WriteLine("Type 1 for computer");
			Console.WriteLine("Type 2 for you");

			first = Int32.Parse(Console.ReadLine());

			isComputer = (first == 1);

			while (this.IsGameFinished() == NotFinished)
			{
				int x, y;
				if (isComputer)
				{
					if (firstStep)
					{
						Random rand = new Random();
						x = rand.Next(3);
						y = rand.Next(3);
						this.Add(x, y, isComputer);
					}
					else
					{
						var move = this.AlphaBetaSearch(this.board, 0, isComputer, new StepValue(0, Int32.MinValue, board), new StepValue(0, Int32.MaxValue, board));
						this.board = move.Move;
					}
					

					Console.WriteLine("Computer Input:");
				}
				else
				{
					do
					{
						Console.WriteLine("Player Input:");
						x = Int32.Parse(Console.ReadLine());
						y = Int32.Parse(Console.ReadLine());
					}
					while (!this.Add(x, y, isComputer));
				}

				isComputer = !isComputer;
				firstStep = false;
				this.PrintBoard();
				Console.WriteLine();
			}

			switch (this.IsGameFinished())
			{
				case 0: Console.WriteLine("Draw!"); break;
				case 1: Console.WriteLine("Computer win!"); break;
				case -1: Console.WriteLine("You win!"); break;
			}

		}
	}
}
