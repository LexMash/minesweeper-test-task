namespace Minesweeper.Board
{
    public readonly struct CellOpenResult
    {
        public readonly int Index;
        public readonly int MinesNearby;

        public CellOpenResult(int index, int minesNearby)
        {
            Index = index;
            MinesNearby = minesNearby;
        }
    }
}
