using Minesweeper.Board;
using System;

namespace Minesweeper.Randomizer
{
    public class CellShuffler : ICellShuffler
    {
        private readonly Random rng;

        public CellShuffler(int seed)
        {
            rng = new Random(seed);
        }

        public void Shuffle(Cell[] cells)
        {
            int n = cells.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (cells[n], cells[k]) = (cells[k], cells[n]);
            }
        }
    }
}
