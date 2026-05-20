using Minesweeper.Data;
using System;
using System.Collections.Generic;

namespace Minesweeper.States
{
    public class GameStateMachine : IStateSwitcher, IDisposable
    {
        private readonly Dictionary<GameStateType, IState> stateMap = new();
        private readonly GameplayContext context;
        private IState currentState;

        public GameStateMachine(GameplayContext context)
        {
            this.context = context;
        }

        public GameStateMachine RegisterState(GameStateType type, IState state)
        {
            if (!stateMap.ContainsKey(type))
                stateMap.Add(type, state);
            else throw new ArgumentException($"State with type {type} is already registered");

            return this;
        }

        public void SwitchStateTo(GameStateType stateType)
        {
            currentState?.Exit();

            if (!stateMap.TryGetValue(stateType, out IState state))
                throw new NullReferenceException($"State with type {stateType} is not registered");

            currentState = state;
            context.UpdateCurrentState(stateType);
            currentState.Enter();
        }

        public void Update(float deltaTime) => currentState?.Update(deltaTime);

        public void Dispose() => currentState?.Exit();
    }
}
