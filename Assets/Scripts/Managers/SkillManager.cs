using MynetDemo.Data;
using MynetDemo.Core;
using MynetDemo.Game;
using UnityEngine;

namespace MynetDemo.Manager
{
    public class SkillManager : SingletonComponent<SkillManager>
    {
        [SerializeField] GameObject _projectile;

        public IRangedAttack RangedAttack { get; set; }

        public delegate void OnAttackEvent();
        public static OnAttackEvent OnAttack;

        void OnEnable()
        {
            CharacterSkill.OnSkill += OnSkill;
            GameFlowManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            CharacterSkill.OnSkill -= OnSkill;
            GameFlowManager.OnGameStateChange -= OnGameStateChange;
        }

        void Update()
        {
            if (GameFlowManager.Instance.State == GameFlowManager.GameState.PLAY)
            {
                if (RangedAttack.UpdateTimer(Time.deltaTime))
                {
                    OnAttack?.Invoke();
                }
            }
        }

        void OnSkill(SkillCode code)
        {
            ISkillStrategy skillStrategy = null;
            switch (code)
            {
                case SkillCode.TripleShot:
                    skillStrategy = new UpgradeToSkillOne();
                    break;
                case SkillCode.SequentialShot:
                    skillStrategy = new UpgradeToSkillTwo();
                    break;
                case SkillCode.DecreaseCooldown:
                    skillStrategy = new UpgradeAttackSpeed(new AdditionModifier(-1f, "Decrease Cooldown Skill"));
                    break;
                case SkillCode.IncreaseArrowSpeed:
                    skillStrategy = new UpgradeProjectileSpeed(new MultiplicationModifier(0.5f, "Increase Arrow Speed Skill"));
                    break;
                case SkillCode.Clone:
                    skillStrategy = new CloneSkill();
                    break;
            }
            Skill skill = new Skill(skillStrategy);
            skill.Perform();
        }

        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (oldState == GameFlowManager.GameState.MENU && newState == GameFlowManager.GameState.TRANSITION)
            {
                RangedAttack = new DefaultRangedAttack(_projectile, 2f, 7f);
            }
            if (oldState == GameFlowManager.GameState.TRANSITION && newState == GameFlowManager.GameState.MENU)
            {
                Destroy(GameObject.Find("Cloned Character"));
            }
        }
    }
}

