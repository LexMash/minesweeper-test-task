using Minesweeper.Board;
using Minesweeper.Configs;
using Minesweeper.Data;
using Minesweeper.Randomizer;
using Minesweeper.States;
using Minesweeper.UI;
using Minesweeper.View;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private GameConfig config;
        [SerializeField] private GameResources resources;
        [SerializeField] private CellVisualConfig visualConfig;

        [Header("UI Views")]
        [SerializeField] private MainMenuScreenView mainMenu;
        [SerializeField] private GameplayScreen gameplayScreen;
        [SerializeField] private GameplayTopPanelView topPanel;
        [SerializeField] private PauseScreenView pauseScreen;
        [SerializeField] private BoardGridView boardGridView;
        [SerializeField] private CounterView timerView;
        [SerializeField] private CounterView mineCounterView;
        [SerializeField] private GameOverScreenView gameOverScreenView;

        private readonly Stack<IDisposable> disposables = new(8);
        private readonly GameplayContext context = new();

        private GameStateMachine stateMachine;

        private void Start()
        {
            var cellShuffler = new CellShuffler(Guid.NewGuid().GetHashCode());

            var boardService = new BoardService(cellShuffler);
            disposables.Push(boardService);

            var cellFactory = new CellViewFactory(resources.CellPrefab);
            disposables.Push(cellFactory);

            var board = new BoardPresenter(boardService, boardGridView, cellFactory, visualConfig);
            disposables.Push(board);
            var timer = new GameTimer(timerView);

            var mineCounter = new MineCounterPresenter(board, mineCounterView);
            mineCounter.Initialize();
            disposables.Push(mineCounter);

            var gameOverScreen = new GameOverScreenPresenter(gameOverScreenView, resources);
            gameOverScreen.Initialize();
            disposables.Push(gameOverScreen);

            stateMachine = new GameStateMachine(context);
            stateMachine
                .RegisterState(GameStateType.MainMenu, new MainMenuState(stateMachine, mainMenu, gameplayScreen))
                .RegisterState(GameStateType.GameplayInitialization,
                    new GameplayInitializationState(stateMachine, board, config, context, timer, mineCounter))

                .RegisterState(GameStateType.GameplayReady, new GameplayReadyState(stateMachine, board, gameplayScreen, topPanel))
                .RegisterState(GameStateType.Gameplay, new GameplayState(stateMachine, board, timer, context, topPanel, gameplayScreen))
                .RegisterState(GameStateType.Pause, new PauseGameState(stateMachine, pauseScreen, context))
                .RegisterState(GameStateType.Restart, new RestartGameState(stateMachine, board, context, timer, mineCounter))
                .RegisterState(GameStateType.GameOver, new GameOverState(stateMachine, context, gameOverScreen));

            stateMachine.SwitchStateTo(GameStateType.MainMenu);

            disposables.Push(stateMachine);
        }

        private void Update() => stateMachine.Update(Time.deltaTime);

        private void OnDestroy()
        {
            while (disposables.Count > 0)
                disposables.Pop().Dispose();
        }
    }
}
