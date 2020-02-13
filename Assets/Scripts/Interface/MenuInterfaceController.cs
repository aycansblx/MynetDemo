using DG.Tweening;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Interface
{
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

        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (newState == GameFlowManager.GameState.MENU)
            {
                EnterScreen();
            }
        }

        void EnterScreen()
        {
            _transform.DOAnchorPosX(0f, 1f);
        }

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
