using Minesweeper.Board;
using Minesweeper.Data;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class RestartGameState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly BoardPresenter board;
        private readonly GameplayContext context;
        private readonly GameTimer timer;
        private readonly MineCounterPresenter mineCounter;

        public RestartGameState(
            IStateSwitcher stateSwitcher, 
            BoardPresenter board, 
            GameplayContext context, 
            GameTimer timer, 
            MineCounterPresenter mineCounter)
        {
            this.stateSwitcher = stateSwitcher;
            this.board = board;
            this.context = context;
            this.timer = timer;
            this.mineCounter = mineCounter;
        }

        public void Enter()
        {
            context.IsWin = false;
            board.Reset();
            timer.Reset();
            mineCounter.Reset();

            stateSwitcher.SwitchStateTo(GameStateType.GameplayReady);
        }

        public void Exit(){}

        public void Update(float dt){}
    }
}
