using Minesweeper.UI;

namespace Minesweeper.States
{
    public class MainMenuState : IState
    {
        private readonly IStateSwitcher stateSwitcher;
        private readonly MainMenuScreenView view;

        public MainMenuState(IStateSwitcher stateSwitcher, MainMenuScreenView view)
        {
            this.stateSwitcher = stateSwitcher;
            this.view = view;
        }

        public void Enter()
        {
            view.StartBttn.onClick.AddListener(StartGame);
            view.Show();
        }

        public void Exit()
        {
            view.StartBttn.onClick.RemoveListener(StartGame);
            view.Hide();
        }

        public void Update(float dt){}
        private void StartGame() => stateSwitcher.SwitchStateTo(GameStateType.GameplayInitialization);
    }
}
