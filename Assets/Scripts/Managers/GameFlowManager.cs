using MynetDemo.Core;
using UnityEngine;

namespace MynetDemo.Manager
{
    public class GameFlowManager : SingletonComponent<GameFlowManager>
    {
        public GameState State { get; private set; } = GameState.NONE;

        public delegate void GameStateChangeEvent(GameState oldState, GameState newState);
        public static GameStateChangeEvent OnGameStateChange;

        void Start()
        {
            SetState(GameState.MENU);
            SetState(GameState.TRANSITION);
        }

        void Update()
        {
            if (State == GameState.PLAY)
            {
                // ... update timers ... //
            }
        }

        public void SetState(GameState newState)
        {
            GameState oldState = State;
            State = newState;
            OnGameStateChange?.Invoke(oldState, State);
        }

        public enum GameState
        {
            NONE, MENU, PLAY, PAUSE, TRANSITION,
        }
    }
}
