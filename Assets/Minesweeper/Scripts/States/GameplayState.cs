using Minesweeper.Board;
using Minesweeper.Data;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class GameplayState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly BoardPresenter board;    
        private readonly GameTimer timer;
        private readonly GameplayContext context;
        private readonly GameplayTopPanelView topPanel;
        private readonly GameplayScreen gameplayScreen;

        public GameplayState(
            IStateSwitcher stateSwitcher, 
            BoardPresenter board,
            GameTimer timer,
            GameplayContext context,
            GameplayTopPanelView topPanel,
            GameplayScreen gameplayScreen)
        {
            this.stateSwitcher = stateSwitcher;
            this.board = board;
            this.timer = timer;
            this.context = context;
            this.topPanel = topPanel;
            this.gameplayScreen = gameplayScreen;
        }

        public void Enter()
        {
            board.AllSafeCellsRevealed += WinGame;
            board.MineClicked += SwitchStateToGameOver;

            topPanel.PauseBttn.onClick.AddListener(PauseGame);
            topPanel.RestartBttn.onClick.AddListener(RestartGame);

            gameplayScreen.Show();
        }

        public void Exit()
        {
            board.AllSafeCellsRevealed -= WinGame;
            board.MineClicked -= SwitchStateToGameOver;

            topPanel.PauseBttn.onClick.RemoveListener(PauseGame);
            topPanel.RestartBttn.onClick.RemoveListener(RestartGame);
        }

        public void Update(float dt) => timer.Execute(dt);

        private void WinGame()
        {
            context.IsWin = true;
            SwitchStateToGameOver();
        }

        private void RestartGame()
        {
            stateSwitcher.SwitchStateTo(GameStateType.Restart);
        }

        private void PauseGame()
        {
            gameplayScreen.Hide();
            stateSwitcher.SwitchStateTo(GameStateType.Pause);
        }

        private void SwitchStateToGameOver()
        {
            stateSwitcher.SwitchStateTo(GameStateType.GameOver);
        }
    }
}
