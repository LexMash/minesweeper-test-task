using Minesweeper.Randomizer;
using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Minesweeper.Board
{
    public class BoardService : IDisposable
    {
        private const int MAX_CELLS_NEARBY = 8;

        //TL, T, TR, L, R, BL, B, BR
        private static readonly int[] deltaRow =    { -1, -1, -1, 0, 0, 1, 1, 1 };
        private static readonly int[] deltaColumn = { -1, 0, 1, -1, 1, -1, 0, 1 };

        private readonly ObjectPool<List<int>> listPool;
        private readonly HashSet<int> openedCells = new(32);
        private readonly List<CellOpenResult> openResults = new(16);

        private readonly ICellShuffler shuffler;

        private Cell[] cells;     
        private int cols;
        private int rows;
        private int mines;

        public BoardService(ICellShuffler shuffler)
        {
            listPool = new ObjectPool<List<int>>(
                    createFunc: () => new List<int>(8),
                    actionOnGet: (instance) => instance.Clear(),
                    actionOnRelease: null,
                    actionOnDestroy: (instance) => instance.Clear(),
                    false,
                    4);

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
            listPool.Dispose();
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

        public IReadOnlyList<CellOpenResult> OpenCell(int cellIndex)
        {
            openResults.Clear();

            ref var cell = ref cells[cellIndex];
            cell.State = CellState.Opened;

            openedCells.Add(cellIndex);
            
            List<int> nearbyCells = listPool.Get();
            SetNearbyCellsToList(cellIndex, nearbyCells);
            var minesNearby = CountNearbyMines(nearbyCells);

            openResults.Add(new CellOpenResult(cellIndex, minesNearby));

            if (minesNearby == 0)
                OpenAllFreeCellsNearby(nearbyCells);

            listPool.Release(nearbyCells);

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

        public int CountNearbyMines(List<int> nearbyCells)
        {
            var mineCount = 0;
            var count = nearbyCells.Count;
            for (int i = 0; i < count; i++)
            {
                var index = nearbyCells[i];
                if (cells[index].HasMine)
                    mineCount++;
            }

            return mineCount;
        }

        public bool AllFreeCellsOpened() => openedCells.Count == cells.Length - mines;

        private void SetNearbyCellsToList(int cellIndex, List<int> list)
        {
            int row = cellIndex / cols;
            int col = cellIndex % cols;

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
                    list.Add(index);
                }              
            }
        }

        private void OpenAllFreeCellsNearby(List<int> nearbyCells)
        {
            var count = nearbyCells.Count;

            for (int i = 0; i < count; i++)
            {
                var cellIndex = nearbyCells[i];

                if (openedCells.Contains(cellIndex))
                    continue;

                ref var cell = ref cells[cellIndex];

                if (cell.HasMine || cell.State == CellState.Marked)
                    continue;

                cell.State = CellState.Opened;
                openedCells.Add(cellIndex);
                List<int> nextNearCells = listPool.Get();
                SetNearbyCellsToList(cellIndex, nextNearCells);
                int minesNearby = CountNearbyMines(nextNearCells);
                openResults.Add(new CellOpenResult(cellIndex, minesNearby));

                if (minesNearby == 0)
                {
                    OpenAllFreeCellsNearby(nextNearCells);
                }

                listPool.Release(nextNearCells);
            }
        }

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
