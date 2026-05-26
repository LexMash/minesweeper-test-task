using Minesweeper.Randomizer;
using System;
using System.Collections.Generic;

namespace Minesweeper.Board
{
    public class BoardService : IDisposable
    {
        private const int MAX_CELLS_NEARBY = 8;

        //TL, T, TR, L, R, BL, B, BR
        private static readonly int[] deltaRow =    { -1, -1, -1, 0, 0, 1, 1, 1 };
        private static readonly int[] deltaColumn = { -1, 0, 1, -1, 1, -1, 0, 1 };

        private readonly List<int> nearIndexes = new(8);
        private readonly HashSet<int> openedCells = new(32);
        private readonly List<CellOpenResult> openResults = new(16);

        private readonly ICellShuffler shuffler;
        private readonly Queue<int> cellQueue = new(32);

        private Cell[] cells;     
        private int cols;
        private int rows;
        private int mines;

        public BoardService(ICellShuffler shuffler)
        {
            this.shuffler = shuffler;
        }

        public void Setup(int rows, int cols, int mines)
        {
            this.cols = cols;
            this.rows = rows;
            this.mines = mines;

            cells = CreateNewCells();
            shuffler.Shuffle(cells);
            openedCells.Clear();
        }

        public void Dispose()
        {
            cells = null;
        }

        public void Reset()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                ref Cell cell = ref cells[i];
                cell.HasMine = i < mines;
                cell.State = CellState.Closed;
            }

            openedCells.Clear();
            shuffler.Shuffle(cells);
        }

        public IReadOnlyList<CellOpenResult> OpenCell(int targetCellIndex)
        {
            openResults.Clear();
            cellQueue.Clear();
            nearIndexes.Clear();

            cellQueue.Enqueue(targetCellIndex);
            openedCells.Add(targetCellIndex);

            while (cellQueue.TryDequeue(out int cellIndex))
            {
                ref var cell = ref cells[cellIndex];
                cell.State = CellState.Opened;        

                int row = cellIndex / cols;
                int col = cellIndex % cols;
                int mineCounter = 0;

                for (int i = 0; i < MAX_CELLS_NEARBY; i++)
                {
                    int newRow = row + deltaRow[i];
                    int newCol = col + deltaColumn[i];

                    if (newRow >= 0 &&
                        newRow < rows &&
                        newCol >= 0 &&
                        newCol < cols)
                    {
                        var index = newRow * cols + newCol;
                        Cell cellN = cells[index];
                        
                        if (cellN.HasMine)
                            mineCounter++;
                        else if (cellN.State == CellState.Closed)
                            nearIndexes.Add(index);
                    }
                }

                openResults.Add(new CellOpenResult(cellIndex, mineCounter));

                if (mineCounter == 0)
                {
                    var count = nearIndexes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var ni = nearIndexes[i];
                        if (!openedCells.Contains(ni))
                        {
                            openedCells.Add(ni);
                            cellQueue.Enqueue(ni);
                        }   
                    }                  
                }

                nearIndexes.Clear();
            }

            return openResults;
        }

        public void MarkCell(int cellIndex)
        {
            ref Cell cell = ref cells[cellIndex];
            cell.State = cell.HasMine 
                ? CellState.Marked 
                : CellState.WrongMark;
        }

        public void UnmarkCell(int cellIndex)
        {
            ref Cell cell = ref cells[cellIndex];
            cell.State = CellState.Closed;
        }

        public Cell GetCell(int cellIndex) => cells[cellIndex];

        public void MoveMineToFreeCell(int cellWithMine)
        {
            ref Cell minedCell = ref cells[cellWithMine];
            minedCell.HasMine = false;
            int size = cells.Length;
            for (int i = 0; i < size; i++)
            {
                if (cellWithMine == i)
                    continue;

                ref Cell cell = ref cells[i];

                if (!cell.HasMine)
                {
                    cell.HasMine = true;
                    return;
                }
            }
        }

        public bool AllFreeCellsOpened() => openedCells.Count == cells.Length - mines;

        private Cell[] CreateNewCells()
        {
            var size = cols * rows;
            Cell[] cells = new Cell[size];
            
            for (int i = 0; i < size; i++)
                cells[i] = new Cell() 
                { 
                    HasMine = i < mines, 
                    State = CellState.Closed 
                };

            return cells;
        }
    }
}
