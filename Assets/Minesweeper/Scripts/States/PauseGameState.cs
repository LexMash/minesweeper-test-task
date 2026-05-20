using Minesweeper.Data;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class PauseGameState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly PauseScreenView pauseScreen;
        private readonly GameplayContext context;

        public PauseGameState(
            IStateSwitcher stateSwitcher,
            PauseScreenView pauseScreen,
            GameplayContext context)
        {
            this.stateSwitcher = stateSwitcher;
            this.pauseScreen = pauseScreen;
            this.context = context;
        }

        public void Enter()
        {
            pauseScreen.RestartBttn.onClick.AddListener(RestartGame);
            pauseScreen.MainMenuBttn.onClick.AddListener(GoToMainMenu);
            pauseScreen.ContinueBttn.onClick.AddListener(ContinueGame);
            pauseScreen.Show();
        }

        public void Exit()
        {
            pauseScreen.RestartBttn.onClick.RemoveListener(RestartGame);
            pauseScreen.MainMenuBttn.onClick.RemoveListener(GoToMainMenu);
            pauseScreen.ContinueBttn.onClick.RemoveListener(ContinueGame);
            pauseScreen.Hide();
        }

        public void Update(float dt) { }

        private void RestartGame() => stateSwitcher.SwitchStateTo(GameStateType.Restart);
        private void GoToMainMenu() => stateSwitcher.SwitchStateTo(GameStateType.MainMenu);
        private void ContinueGame() => stateSwitcher.SwitchStateTo(context.PreviousState);
    }
}
