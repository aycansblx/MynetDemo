using DG.Tweening;
using MynetDemo.Manager;
using MynetDemo.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MynetDemo.Interface
{
    public class GameInterfaceController : MonoBehaviour
    {
        [SerializeField] RectTransform _exitButton;

        [SerializeField] GameObject _button;

        [SerializeField] CharacterSkillSet _skillSet;

        RectTransform _transform;

        readonly List<Button> _skillButtons = new List<Button>();

        void Awake()
        {
            _transform = GetComponent<RectTransform>();

            for (int i = 0; i < _skillSet.Skills.Count; i++)
            {
                CreateSkillButton(i, _skillSet.Skills[i]);
            }
        }

        void OnEnable()
        {
            GameFlowManager.OnGameStateChange += OnGameStateChange;
            SkillManager.OnSkillCast += OnSkillCast;
        }

        void OnDisable()
        {
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
            SkillManager.OnSkillCast -= OnSkillCast;
        }

        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (newState == GameFlowManager.GameState.PLAY)
            {
                EnterScreen();
                SetAllButtonInteractivities(true);
            }
        }

        void OnSkillCast(int order, int skillsCastedSoFar)
        {
            _skillButtons[order].interactable = false;
            if(skillsCastedSoFar == 3)
            {
                SetAllButtonInteractivities(false);
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

        void SetAllButtonInteractivities(bool value)
        {
            foreach (Button skillButton in _skillButtons)
            {
                skillButton.interactable = value;
            }
        }

        void CreateSkillButton(int order, CharacterSkill skill)
        {
            GameObject skillButton = Instantiate(_button, _transform);

            skillButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(order % 5 * 90f, order / 5 * 90f);

            Button buttonComponent = skillButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(delegate { SkillButtonBehaviour(order, skill); });

            skillButton.GetComponent<Image>().sprite = skill.Image;

            _skillButtons.Add(buttonComponent);
        }

        void SkillButtonBehaviour(int order, CharacterSkill skill)
        {
            SkillManager.Instance.CastSkill(order, skill);
        }
    }
}
