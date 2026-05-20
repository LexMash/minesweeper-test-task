using System;

namespace Minesweeper.Board
{
    public interface ICellMarkNotifier
    {
        event Action CellMarked;
        event Action CellUnmarked;
    }
}