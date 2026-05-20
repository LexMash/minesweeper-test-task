using Minesweeper.Configs;
using System;

namespace Minesweeper.UI
{
    public class GameOverScreenPresenter : IDisposable
    {
        private readonly GameOverScreenView view;
        private readonly GameResources resources;

        public event Action Restart;
        public event Action ToMainMenu;

        public GameOverScreenPresenter(
            GameOverScreenView view, 
            GameResources resources)
        {
            this.view = view;
            this.resources = resources;
        }

        public void Initialize()
        {
            view.RestartBttn.onClick.AddListener(RestartInvoke);
            view.MainMenuBttn.onClick.AddListener(ToMainMenuInvoke);
        }

        public void ShowWin()
        {
            view.Setup(resources.WinMessage, resources.WinImage);
            view.Show();
        }

        public void ShowLose()
        {
            view.Setup(resources.LoseMessage, resources.LoseImage);
            view.Show();
        }

        public void Hide() => view.Hide();

        public void Dispose()
        {
            view.RestartBttn.onClick.RemoveListener(RestartInvoke);
            view.MainMenuBttn.onClick.RemoveListener(ToMainMenuInvoke);
        }

        private void RestartInvoke() => Restart?.Invoke();
        private void ToMainMenuInvoke() => ToMainMenu?.Invoke();
    }
}
