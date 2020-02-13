using DG.Tweening;
using MynetDemo.Manager;
using MynetDemo.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MynetDemo.Interface
{
    public class GameInterfaceController : MonoBehaviour
    {
        [SerializeField] RectTransform _exitButton;

        [SerializeField] GameObject _button;

        [SerializeField] CharacterSkillSet _skillSet;

        RectTransform _transform;

        void Awake()
        {
            _transform = GetComponent<RectTransform>();

            for (int i = 0; i < _skillSet.Skills.Count; i++)
            {
                GameObject button = Instantiate(_button, _transform);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(i % 5 * 90f, i / 5 * 90f);
                button.GetComponent<Button>().onClick.AddListener(_skillSet.Skills[i].Activate);
                button.GetComponent<Image>().sprite = _skillSet.Skills[i].Image;
            }
        }

        void OnEnable()
        {
            GameFlowManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
        }

        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (newState == GameFlowManager.GameState.PLAY)
            {
                EnterScreen();
            }
        }

        void EnterScreen()
        {
            _transform.DOAnchorPosY(0f, 1f).OnComplete(() =>
            {
                _exitButton.DOAnchorPosX(-10f, 0.5f);
            });
        }

        public void LeaveScreen()
        {
            GameFlowManager.Instance.SetState(GameFlowManager.GameState.TRANSITION);
            _exitButton.DOAnchorPosX(145f, 0.5f).OnComplete(() =>
            {
                _transform.DOAnchorPosY(-800f, 3f);
            });
        }
    }
}
