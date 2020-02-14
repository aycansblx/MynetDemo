using DG.Tweening;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Game
{
    /// <summary>
    /// This class is the main controller of the character.
    /// </summary>
    public class Character : MonoBehaviour
    {
        const float _LINEAR_SPEED_ = 3f;
        const float _ANGULAR_SPEED_ = 180f;

        [SerializeField] Vector3 _inScreenPosition;
        [SerializeField] Vector3 _outScreenPosition;

        void OnEnable()
        {
            GameFlowManager.OnGameStateChange += OnGameStateChange;
            SkillManager.OnAttack += OnAttack;
        }

        void OnDisable()
        {
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
            SkillManager.OnAttack -= OnAttack;
        }

        /// <summary>
        /// Registered method to GameFlowManager's OnGameStateChange event.
        /// </summary>
        /// <param name="oldState">The previous game state.</param>
        /// <param name="newState">The current game state.</param>
        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (oldState == GameFlowManager.GameState.MENU && newState == GameFlowManager.GameState.TRANSITION)
            {
                EnterScreen();
            }

            if (oldState == GameFlowManager.GameState.PLAY && newState == GameFlowManager.GameState.TRANSITION)
            {
                LeaveScreen();
            }
        }

        /// <summary>
        /// Registered method to SkillManager's OnAttack event.
        /// </summary>
        void OnAttack()
        {
            SkillManager.Instance.RangedAttack.Shoot(transform.position, transform.eulerAngles.x);
        }

        /// <summary>
        /// This function turns the character's face to a given world position.
        /// </summary>
        /// <param name="worldPosition">The position where the character will look at.</param>
        void FaceTo(Vector3 worldPosition)
        {
            Vector3 diff = worldPosition - transform.position;
            float degree = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            transform.DORotateQuaternion(Quaternion.Euler(degree, -90f, 90f), Mathf.Abs(degree) / _ANGULAR_SPEED_);
        }

        /// <summary>
        /// Calculates the traverse duration for a given destination.
        /// </summary>
        /// <param name="worldPosition">The destination of the traverse.</param>
        /// <returns></returns>
        float GetTraverseDuration(Vector3 worldPosition)
        {
            return Vector3.Distance(transform.position, worldPosition) / _LINEAR_SPEED_;
        }

        /// <summary>
        /// When called, the character enters the screen and then changes the game state.
        /// </summary>
        void EnterScreen()
        {
            FaceTo(_inScreenPosition);
            transform.DOMove(_inScreenPosition, GetTraverseDuration(_inScreenPosition)).OnComplete(() => {
                GameFlowManager.Instance.SetState(GameFlowManager.GameState.PLAY);
            });
        }

        /// <summary>
        /// When called, the character leaves the screen and then changes the game state.
        /// </summary>
        void LeaveScreen()
        {
            FaceTo(_outScreenPosition);
            transform.DOMove(_outScreenPosition, GetTraverseDuration(_outScreenPosition)).OnComplete(() => {
                GameFlowManager.Instance.SetState(GameFlowManager.GameState.MENU);
            });
        }
    }
}
