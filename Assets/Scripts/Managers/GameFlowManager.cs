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
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SetState(GameState.TRANSITION);
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
