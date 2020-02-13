using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Game
{
    public class Character : MonoBehaviour
    {
        const float _LINEAR_SPEED_ = 5f;
        const float _ANGULAR_SPEED_ = 360f;

        [SerializeField] Vector3 _inScreenPosition;
        [SerializeField] Vector3 _outScreenPosition;

        void OnEnable()
        {
            GameFlowManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
        }

        void Update()
        {

        }

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

        void FaceTo(Vector3 worldPosition)
        {
            Vector3 diff = worldPosition - transform.position;
            float degree = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = degree;

            transform.DORotate(eulerAngles, Mathf.Abs(degree) / _ANGULAR_SPEED_);
        }

        float GetTraverseDuration(Vector3 worldPosition)
        {
            return Vector3.Distance(transform.position, worldPosition) / _LINEAR_SPEED_;
        }

        void EnterScreen()
        {
            FaceTo(_inScreenPosition);
            transform.DOMove(_inScreenPosition, GetTraverseDuration(_inScreenPosition)).OnComplete(() => {
                GameFlowManager.Instance.SetState(GameFlowManager.GameState.PLAY);
            });
        }

        void LeaveScreen()
        {
            FaceTo(_outScreenPosition);
            transform.DOMove(_outScreenPosition, GetTraverseDuration(_outScreenPosition)).OnComplete(() => {
                GameFlowManager.Instance.SetState(GameFlowManager.GameState.MENU);
            });
        }
    }
}
