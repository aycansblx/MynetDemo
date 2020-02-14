using DG.Tweening;
using MynetDemo.Manager;
using MynetDemo.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MynetDemo.Interface
{
    /// <summary>
    /// A very simple game interface controller...
    /// Controls the interface of the PLAY phase.
    /// </summary>
    public class GameInterfaceController : MonoBehaviour
    {
        [SerializeField] GameObject _buttonPrefab;

        [SerializeField] RectTransform _exitButton;

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

        /// <summary>
        /// Registered method to GameFlowManager's OnGameStateChange event.
        /// </summary>
        /// <param name="oldState">The previous game state.</param>
        /// <param name="newState">The current game state.</param>
        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (newState == GameFlowManager.GameState.PLAY)
            {
                EnterScreen();
                SetButtonActivities(true);
            }
        }

        /// <summary>
        /// Registered method to SkillManagers's OnSkillCast event.
        /// </summary>
        /// <param name="order">The order of the casted skill (in the skill set)</param>
        /// <param name="skillsCastedSoFar">Skills casted so far.</param>
        void OnSkillCast(int order, int skillsCastedSoFar)
        {
            _skillButtons[order].interactable = false;
            if(skillsCastedSoFar == 3)
            {
                SetButtonActivities(false);
            }
        }

        /// <summary>
        /// When called, Game Interface enters the screen.
        /// When finished, exit button is triggered to enter the screen.
        /// </summary>
        void EnterScreen()
        {
            _transform.DOAnchorPosY(0f, 1f).OnComplete(() =>
            {
                _exitButton.DOAnchorPosX(-10f, 0.5f);
            });
        }

        /// <summary>
        /// When called, exit button leaves the screen first.
        /// Then it triggers the whole interface to leave the screen.
        /// </summary>
        public void LeaveScreen()
        {
            GameFlowManager.Instance.SetState(GameFlowManager.GameState.TRANSITION);
            _exitButton.DOAnchorPosX(145f, 0.5f).OnComplete(() =>
            {
                _transform.DOAnchorPosY(-800f, 3f);
            });
        }

        /// <summary>
        /// Sets button activities with respect to the given param.
        /// </summary>
        /// <param name="value">Provide true if you want all buttons interactable :)</param>
        void SetButtonActivities(bool value)
        {
            foreach (Button skillButton in _skillButtons)
            {
                skillButton.interactable = value;
            }
        }

        /// <summary>
        /// Creates skill button.
        /// </summary>
        /// <param name="order">Order of the skill in the skill set.</param>
        /// <param name="skill">Skill data.</param>
        void CreateSkillButton(int order, CharacterSkill skill)
        {
            GameObject skillButton = Instantiate(_buttonPrefab, _transform);

            skillButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(order % 5 * 90f, order / 5 * 90f);

            Button buttonComponent = skillButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(delegate { SkillManager.Instance.CastSkill(order, skill); });

            skillButton.GetComponent<Image>().sprite = skill.Image;

            _skillButtons.Add(buttonComponent);
        }
    }
}
