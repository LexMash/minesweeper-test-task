using Minesweeper.Data;
using Minesweeper.UI;

namespace Minesweeper.States
{
    public class GameOverState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly GameplayContext context;
        private readonly GameOverScreenPresenter gameOverScreen;

        public GameOverState(IStateSwitcher stateSwitcher, GameplayContext context, GameOverScreenPresenter gameOverScreen)
        {
            this.stateSwitcher = stateSwitcher;
            this.context = context;
            this.gameOverScreen = gameOverScreen;
        }

        public void Enter()
        {
            if (context.IsWin)
                gameOverScreen.ShowWin();
            else
                gameOverScreen.ShowLose();

            gameOverScreen.Restart += RestartGame;
            gameOverScreen.ToMainMenu += GoToMainMenu;
        }

        public void Exit()
        {
            gameOverScreen.Restart -= RestartGame;
            gameOverScreen.ToMainMenu -= GoToMainMenu;
            gameOverScreen.Hide();
        }

        public void Update(float dt){}

        private void GoToMainMenu() => stateSwitcher.SwitchStateTo(GameStateType.MainMenu);
        private void RestartGame() => stateSwitcher.SwitchStateTo(GameStateType.Restart);
    }
}
