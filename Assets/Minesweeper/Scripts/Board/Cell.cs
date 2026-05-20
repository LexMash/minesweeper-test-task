using System;

namespace Minesweeper.Board
{
    [Serializable]
    public struct Cell
    {    
        public bool HasMine;
        public CellState State;
    }
}
