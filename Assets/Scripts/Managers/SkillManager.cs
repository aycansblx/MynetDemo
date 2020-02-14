using MynetDemo.Data;
using MynetDemo.Core;
using MynetDemo.Game;
using UnityEngine;

namespace MynetDemo.Manager
{
    /// <summary>
    /// Character's skill manager.
    /// We attach this to character object.
    /// If the character is cloned, remove it from the cloned object (since it is a singleton).
    /// </summary>
    public class SkillManager : SingletonComponent<SkillManager>
    {
        [SerializeField] float _startingAttackSpeed;
        [SerializeField] float _startingArrowSpeed;

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

        /// <summary>
        /// When a skill is casted this function is called.
        /// </summary>
        /// <param name="order">Order of the skill in the skill set.</param>
        /// <param name="characterSkill">Skill data</param>
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

        /// <summary>
        /// Registered method to GameFlowManager's OnGameStateChange event.
        /// </summary>
        /// <param name="oldState">The previous game state.</param>
        /// <param name="newState">The current game state.</param>
        void OnGameStateChange(GameFlowManager.GameState oldState, GameFlowManager.GameState newState)
        {
            if (oldState == GameFlowManager.GameState.MENU && newState == GameFlowManager.GameState.TRANSITION)
            {
                RangedAttack = new DefaultRangedAttack(_projectile, _startingAttackSpeed, _startingArrowSpeed);
            }
            if (oldState == GameFlowManager.GameState.TRANSITION && newState == GameFlowManager.GameState.MENU)
            {
                Clean();
            }
        }

        /// <summary>
        /// Cleans everything related to skill usages.
        /// </summary>
        void Clean()
        {
            Character[] characters = FindObjectsOfType<Character>();
            if (characters.Length > 1)
            {
                foreach (Character character in characters)
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
