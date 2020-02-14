using MynetDemo.Data;
using MynetDemo.Core;
using MynetDemo.Game;
using UnityEngine;

namespace MynetDemo.Manager
{
    public class SkillManager : SingletonComponent<SkillManager>
    {
        [SerializeField] GameObject _projectile;

        int _skillsCastedSoFar;

        public IRangedAttack RangedAttack { get; set; }

        public delegate void OnAttackEvent();
        public static OnAttackEvent OnAttack;

        public delegate void OnSkillCastEvent(int skillOrder, int skillsCastedSoFar);
        public static OnSkillCastEvent OnSkillCast;

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
            if (GameFlowManager.Instance.State == GameFlowManager.GameState.PLAY)
            {
                if (RangedAttack.UpdateTimer(Time.deltaTime))
                {
                    OnAttack?.Invoke();
                }
            }
        }

        public void CastSkill(int order, CharacterSkill characterSkill)
        {
            ISkillStrategy skillStrategy = null;
            switch (characterSkill.Code)
            {
                case SkillCode.TripleShot:
                    skillStrategy = new UpgradeToTripleShot();
                    break;
                case SkillCode.DoubleShot:
                    skillStrategy = new UpgradeToDoubleShot();
                    break;
                case SkillCode.Haste:
                    skillStrategy = new UpgradeAttackSpeed(new AdditionModifier(-1f, characterSkill.Name));
                    break;
                case SkillCode.UberBolt:
                    skillStrategy = new UpgradeProjectileSpeed(new MultiplicationModifier(0.5f, characterSkill.Name));
                    break;
                case SkillCode.Clone:
                    skillStrategy = new CloneTheCharacterRandomly();
                    break;
            }
            Skill skill = new Skill(skillStrategy);
            skill.Perform();
            OnSkillCast?.Invoke(order, ++_skillsCastedSoFar);
        }

        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (oldState == GameFlowManager.GameState.MENU && newState == GameFlowManager.GameState.TRANSITION)
            {
                RangedAttack = new DefaultRangedAttack(_projectile, 2f, 7f);
            }
            if (oldState == GameFlowManager.GameState.TRANSITION && newState == GameFlowManager.GameState.MENU)
            {
                Character[] characters = FindObjectsOfType<Character>();
                if (characters.Length > 1)
                {
                    foreach(Character character in characters)
                    {
                        if (character.transform != transform)
                        {
                            PoolingManager.Instance.Add(character.gameObject);
                        }
                    }
                }
                _skillsCastedSoFar = 0;
            }
        }
    }
}

