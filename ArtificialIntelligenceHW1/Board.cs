using System;
using System.Collections.Generic;
using System.Text;

namespace ArtificialIntelligenceHW1
{
    public enum Directions
	{
        Left,
        Right,
        Up,
        Down
	}

    public class Board
    {
        private int[,] tiles;

        private int size;

        private int zeroY;

        private int zeroX;
        
        public int Manhattan { get; private set; }

        public string BoardToString { get; private set; }

        public List<Directions> Path { get; private set; }

        public Board(int[,] tiles, List<Directions> path, int zeroX, int zeroY)
        {
            this.zeroY = zeroY;
            this.zeroX = zeroX;

            this.tiles = tiles;
            this.Path = path;
            this.size = (int)Math.Sqrt(tiles.Length);
            CalculateManhattan();
        }

        private int CalculateManhattan()
        {
            int value = 0;
            var stringBuilder = new StringBuilder(size * size);
            for (int i = 0; i < size; i++)
            { 
                for (int j = 0; j < size; j++)
                {
                    stringBuilder.Append(tiles[i, j]);
                    if (tiles[i, j] != 0)
                    {
                        int expectedX = Program.ManhantanTable[tiles[i, j]].Item2;
                        int expectedY = Program.ManhantanTable[tiles[i, j]].Item1;
                        value += Math.Abs(expectedX - j);
                        value += Math.Abs(expectedY - i);
                    }
                }
            }
            stringBuilder.Append(value + Path.Count);
            this.BoardToString = stringBuilder.ToString();
            this.Manhattan = value;
            return value;
        }

        public bool IsGoal()
        {
            int x = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (tiles[i, j] != x && tiles[i, j] != 0)
                    {
                        return false;
                    }
                    x++;
                }
            }

            return true;
        }

        public IEnumerable<Board> Neighbors()
        {
            int[,] temp;
            var result = new C5.IntervalHeap<Board>(100, new BoardComparer(Program.ManhattanHeuristic));
            if (zeroX > 0)
			{
                var nextPath = new List<Directions>(Path)
                {
                    Directions.Right
                };

                temp = (int[,])tiles.Clone();
                temp[zeroY, zeroX] = temp[zeroY, zeroX - 1];
                temp[zeroY, zeroX - 1] = 0;
                result.Add(new Board(temp, nextPath, zeroX -1, zeroY));
            }
            if (zeroX+1 < size)
            {
                var nextPath = new List<Directions>(Path)
                {
                    Directions.Left
                };

                temp = (int[,])tiles.Clone();
                temp[zeroY, zeroX] = temp[zeroY, zeroX + 1];
				temp[zeroY, zeroX + 1] = 0;
                result.Add(new Board(temp, nextPath, zeroX +1, zeroY));
            }

            if (zeroY > 0)
            {
				var nextPath = new List<Directions>(Path)
				{
					Directions.Down
				};

				temp = (int[,])tiles.Clone();
                temp[zeroY, zeroX] = temp[zeroY - 1, zeroX];
                temp[zeroY - 1, zeroX] = 0;
                result.Add(new Board(temp, nextPath, zeroX, zeroY - 1));
            }

            if (zeroY + 1 < size)
            {
                var nextPath = new List<Directions>(Path)
                {
                    Directions.Up

                };

                temp = (int[,])tiles.Clone();
                temp[zeroY, zeroX] = temp[zeroY + 1, zeroX];
                temp[zeroY + 1, zeroX] = 0;
                result.Add(new Board(temp, nextPath, zeroX, zeroY + 1));
            }

            return result;
        }

        public bool IsSolvable()
        {
            if(size % 2 == 0)
			{
                return CountInversionsAndFindZero() % 2 != 1;
			}
			else
			{
                return (CountInversionsAndFindZero() + zeroY) % 2 != 1;  
			}
        }


        private int CountInversionsAndFindZero()
		{
            int inversions = 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
                    if (tiles[i,j] != 0)
                    {
                        for (int k = 0; k < j; k++)
                        {
                            if (tiles[i,k] > tiles[i,j])
                            {
                                inversions++;
                            }
                        }
                    }
					else
					{
                        zeroX = j;
                        zeroY = i;
                    }
				}
			}
            return inversions;
		}
    }
}
