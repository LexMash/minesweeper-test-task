using Minesweeper.Board;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class GameplayReadyState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly GameplayScreen gameplayScreen;
        private readonly GameplayTopPanelView topPanel;
        private readonly BoardPresenter board;

        public GameplayReadyState(
            IStateSwitcher stateSwitcher,
            BoardPresenter board,
            GameplayScreen gameplayScreen,
            GameplayTopPanelView topPanel)
        {
            this.stateSwitcher = stateSwitcher;
            this.board = board;
            this.gameplayScreen = gameplayScreen;
            this.topPanel = topPanel;
        }

        public void Enter()
        {
            gameplayScreen.Show();
            board.FirstClickPerformed += OnFirstClickPerformed;
            topPanel.PauseBttn.onClick.AddListener(PauseGame);
        }

        public void Exit()
        {
            board.FirstClickPerformed -= OnFirstClickPerformed;
            topPanel.PauseBttn.onClick.RemoveListener(PauseGame);
        }

        public void Update(float dt){}

        private void OnFirstClickPerformed() => stateSwitcher.SwitchStateTo(GameStateType.Gameplay);
        private void PauseGame() => stateSwitcher.SwitchStateTo(GameStateType.Pause);
    }
}
