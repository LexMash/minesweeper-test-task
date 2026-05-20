using Minesweeper.Board;
using Minesweeper.Configs;
using Minesweeper.Data;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class GameplayInitializationState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly BoardPresenter board;
        private readonly GameConfig config;
        private readonly GameplayContext context;
        private readonly GameTimer timer;
        private readonly MineCounterPresenter mineCounter;

        public GameplayInitializationState(
            IStateSwitcher stateSwitcher, 
            BoardPresenter board, 
            GameConfig config, 
            GameplayContext context, 
            GameTimer timer, 
            MineCounterPresenter mineCounter)
        {
            this.stateSwitcher = stateSwitcher;
            this.board = board;
            this.config = config;
            this.context = context;
            this.timer = timer;
            this.mineCounter = mineCounter;
        }

        public void Enter()
        {
            context.IsWin = false;
            board.Initialize(config.Rows, config.Columns, config.MinesAmount);
            timer.Reset();
            mineCounter.Setup(config.MinesAmount);
            stateSwitcher.SwitchStateTo(GameStateType.GameplayReady);
        }

        public void Exit(){}
        public void Update(float dt){}
    }
}
