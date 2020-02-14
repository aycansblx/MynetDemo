using DG.Tweening;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Interface
{
    /// <summary>
    /// A very simple menu interface implementation...
    /// ... with only a play button :/
    /// </summary>
    public class MenuInterfaceController : MonoBehaviour
    {
        RectTransform _transform;

        void OnEnable()
        {
            GameFlowManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
        }

        void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Registered method to GameFlowManager's OnGameStateChange event.
        /// </summary>
        /// <param name="oldState">The previous game state.</param>
        /// <param name="newState">The current game state.</param>
        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (newState == GameFlowManager.GameState.MENU)
            {
                EnterScreen();
            }
        }

        /// <summary>
        /// When called, interface container enters the screen.
        /// </summary>
        void EnterScreen()
        {
            _transform.DOAnchorPosX(0f, 1f);
        }

        /// <summary>
        /// When called interface container leaves the screen and then changes the game state.
        /// </summary>
        public void LeaveScreen()
        {
            _transform.DOAnchorPosX(-450f, 1f).OnComplete(() =>
            {
                _transform.anchoredPosition = new Vector2(450f, 0f);
                GameFlowManager.Instance.SetState(GameFlowManager.GameState.TRANSITION);
            });
        }
    }
}
