namespace Minesweeper.Board
{
    public enum CellState : byte
    {
        Closed = 0,
        Opened,
        Marked,
        Mined,
        MineBlow,
        WrongMark,
    }
}
