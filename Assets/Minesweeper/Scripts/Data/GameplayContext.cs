using Minesweeper.States;

namespace Minesweeper.Data
{
    public class GameplayContext
    {
        public bool IsWin;
        public GameStateType CurrentState;
        public GameStateType PreviousState;

        public void UpdateCurrentState(GameStateType state)
        {
            PreviousState = CurrentState;
            CurrentState = state;
        }
    }
}
