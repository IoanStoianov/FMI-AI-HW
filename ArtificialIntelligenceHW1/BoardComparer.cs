using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ArtificialIntelligenceHW1
{
	public class BoardComparer : IComparer<Board>
	{
		private Func<Board, int> heuristic;
		public BoardComparer(Func<Board, int> heuristic)
		{
			this.heuristic = heuristic;
		}
		public int Compare([AllowNull] Board x, [AllowNull] Board y)
		{
			return heuristic(x).CompareTo(heuristic(y));
		}
	}
}
