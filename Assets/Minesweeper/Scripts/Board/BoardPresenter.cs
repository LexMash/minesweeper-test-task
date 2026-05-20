using Minesweeper.Configs;
using Minesweeper.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.Board
{
    public class BoardPresenter : IDisposable, ICellMarkNotifier
    {
        private readonly BoardService service;
        private readonly BoardGridView boardView;
        private readonly CellViewFactory cellFactory;
        private readonly CellVisualConfig cellVisualConfig;

        private bool firstClickPerformed;
        private CellView[] cellViews;

        public event Action FirstClickPerformed;
        public event Action AllSafeCellsRevealed;
        public event Action MineClicked;
        public event Action CellMarked;
        public event Action CellUnmarked;

        public BoardPresenter(
            BoardService service,
            BoardGridView boardView,
            CellViewFactory cellFactory,
            CellVisualConfig cellVisualConfig)
        {
            this.service = service;
            this.boardView = boardView;
            this.cellFactory = cellFactory;
            this.cellVisualConfig = cellVisualConfig;
        }

        public void Initialize(int width, int height, int mines)
        {
            CleanupCells();

            service.Setup(width, height, mines);
            boardView.SetupGrid(width, height);

            Sprite closedStateSprite = cellVisualConfig.GetSprite(CellState.Closed);
            Transform cellParent = boardView.Root;
            int cellsAmount = width * height;
            cellViews = new CellView[cellsAmount];

            for (int i = 0; i < cellsAmount; i++)
            {
                CellView cellView = cellFactory.Create();
                cellView.Setup(closedStateSprite, i);
                cellView.SetParent(cellParent);
                cellView.Show();
                cellViews[i] = cellView;

                cellView.OnClick += CellClicked;
            }

            firstClickPerformed = false;
        }

        public void Reset()
        {
            firstClickPerformed = false;
            service.Reset();
            Sprite closedStateSprite = cellVisualConfig.GetSprite(CellState.Closed);

            for (int i = 0; i < cellViews.Length; i++)
            {
                CellView cellView = cellViews[i];
                cellView.UpdateImage(closedStateSprite);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < cellViews.Length; i++)
            {
                CellView cellView = cellViews[i];
                cellView.OnClick -= CellClicked;
            }
        }

        private void CellClicked(MouseClickType click, int index)
        {
            switch (click)
            {
                case MouseClickType.Left:
                    HandleFirstClick(index);
                    HandleCellClick(index);
                    break;

                case MouseClickType.Right:
                    MarkCell(index);
                    break;
            }
        }

        private void MarkCell(int index)
        {
            Cell cell = service.GetCell(index);

            if (cell.State == CellState.Opened)
                return;

            var cellView = cellViews[index];

            if (cell.State == CellState.Closed)
            {
                service.MarkCell(index);
                var sprite = cellVisualConfig.GetSprite(CellState.Marked);
                cellView.UpdateImage(sprite);
                CellMarked?.Invoke();
            }
            else
            {
                service.UnmarkCell(index);
                var sprite = cellVisualConfig.GetSprite(CellState.Closed);
                cellView.UpdateImage(sprite);
                CellUnmarked?.Invoke();
            }
        }

        private void HandleFirstClick(int index)
        {
            if (!firstClickPerformed)
            {
                firstClickPerformed = true;

                Cell cell = service.GetCell(index);

                if (cell.HasMine)
                    service.MoveMineToFreeCell(index);

                FirstClickPerformed?.Invoke();
            }
        }

        private void HandleCellClick(int index)
        {
            Cell cell = service.GetCell(index);

            if (!firstClickPerformed)
            {
                firstClickPerformed = true;

                if (cell.HasMine)
                    service.MoveMineToFreeCell(index);
            }

            if (cell.State == CellState.Opened)
                return;

            if (cell.HasMine)
            {
                OpenAllMines(index);
                MineClicked?.Invoke();
            }
            else
            {
                OpenCells(index);

                if (service.AllFreeCellsOpened())
                {
                    AllSafeCellsRevealed?.Invoke();
                }
            }
        }

        private void OpenAllMines(int mineIndex)
        {
            for (int i = 0; i < cellViews.Length; i++)
            {
                var cell = service.GetCell(i);
                var view = cellViews[i];

                if (cell.HasMine)
                {
                    var state = i == mineIndex
                        ? CellState.MineBlow
                        : CellState.Mined;

                    var sprite = cellVisualConfig.GetSprite(state);
                    view.UpdateImage(sprite);
                }
                else if (cell.State == CellState.WrongMark)
                {
                    var sprite = cellVisualConfig.GetSprite(CellState.WrongMark);
                    view.UpdateImage(sprite);
                }
            }
        }

        private void OpenCells(int index)
        {
            IReadOnlyList<CellOpenResult> results = service.OpenCell(index);
            int count = results.Count;
            for (int i = 0; i < count; i++)
            {
                CellOpenResult result = results[i];
                CellView cellView = cellViews[result.Index];
                Sprite newImage = cellVisualConfig.GetSprite(result.MinesNearby);
                cellView.UpdateImage(newImage);
            }
        }

        private void CleanupCells()
        {
            if (cellViews != null)
            {
                for (int i = 0; i < cellViews.Length; i++)
                {
                    var view = cellViews[i];
                    cellFactory.Release(view);
                }
            }
        }
    }
}
