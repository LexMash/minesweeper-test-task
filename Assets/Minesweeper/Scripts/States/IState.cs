namespace Minesweeper.States
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update(float dt);
    }
}
