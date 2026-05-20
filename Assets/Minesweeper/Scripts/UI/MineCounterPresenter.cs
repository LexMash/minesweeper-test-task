using Minesweeper.Board;
using Minesweeper.View;
using System;

namespace Minesweeper.UI
{
    public class MineCounterPresenter : IDisposable
    {
        private readonly ICellMarkNotifier markNotifier;
        private readonly CounterView view;
        private int counter;
        private int targetAmount;

        public MineCounterPresenter(ICellMarkNotifier markNotifier, CounterView view)
        {
            this.markNotifier = markNotifier;
            this.view = view;;
        }

        public void Initialize()
        {
            markNotifier.CellMarked += OnCellMarked;
            markNotifier.CellUnmarked += OnCellUnmarked;
        }

        public void Setup(int minesAmount)
        {
            targetAmount = minesAmount;
            counter = minesAmount;
            RefreshView();
        }

        public void Reset()
        {
            counter = targetAmount;
            RefreshView();
        }

        public void Dispose()
        {
            markNotifier.CellMarked -= OnCellMarked;
            markNotifier.CellUnmarked -= OnCellUnmarked;
        }

        private void RefreshView() => view.UpdateCounter(counter);

        private void OnCellUnmarked()
        {
            counter++;
            RefreshView();
        }

        private void OnCellMarked()
        {
            counter--;
            RefreshView();
        }
    }
}
