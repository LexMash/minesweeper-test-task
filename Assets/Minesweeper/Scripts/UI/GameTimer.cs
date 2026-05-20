using Minesweeper.View;

namespace Minesweeper.UI
{
    public class GameTimer
    {
        private const int TIME_UPDATE_INTERVAL = 1;

        private readonly CounterView view;
        private float counter;
        private int time;

        public GameTimer(CounterView view)
        {
            this.view = view;
        }

        public void Reset()
        {
            counter = 0;
            time = 0;
            RefreshView();
        }

        public void Execute(float deltaTime)
        {
            counter += deltaTime;

            if (counter >= TIME_UPDATE_INTERVAL)
            {
                time += TIME_UPDATE_INTERVAL;
                counter -= TIME_UPDATE_INTERVAL;
                RefreshView();
            }
        }

        private void RefreshView() => view.UpdateCounter(time);
    }
}
