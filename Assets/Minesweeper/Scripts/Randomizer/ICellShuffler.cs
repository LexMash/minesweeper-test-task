using Minesweeper.Board;

namespace Minesweeper.Randomizer
{
    public interface ICellShuffler
    {
        void Shuffle(Cell[] cells);
    }
}
