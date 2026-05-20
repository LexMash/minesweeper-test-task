namespace Minesweeper.States
{
    public interface IStateSwitcher
    {
        void SwitchStateTo(GameStateType type);
    }
}