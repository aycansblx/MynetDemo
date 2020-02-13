using MynetDemo.Core;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Game
{
    /// <summary>
    /// Skill strategy interface to build an awesome strategy pattern.
    /// </summary>
    public interface ISkillStrategy
    {
        /// <summary>
        /// This method performs skill operations.
        /// </summary>
        void Perform();
    }

    /// <summary>
    /// Skill class.
    /// </summary>
    public class Skill
    {
        public ISkillStrategy Strategy { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="strategy">The strategy (skill logic) of the corresponding skill.</param>
        public Skill(ISkillStrategy strategy)
        {
            Strategy = strategy;
        }

        public void Perform()
        {
            Strategy.Perform();
        }
    }

    /// <summary>
    /// When this strategy is performed, the first skill becomes active.
    /// </summary>
    public class UpgradeToSkillOne : ISkillStrategy
    {
        public void Perform()
        {
            SkillManager.Instance.RangedAttack = new RangedAttackWithSkillOne(SkillManager.Instance.RangedAttack);
        }
    }

    /// <summary>
    /// When this strategy is performed, the second skill becomes active.
    /// </summary>
    public class UpgradeToSkillTwo : ISkillStrategy
    {
        public void Perform()
        {
            SkillManager.Instance.RangedAttack = new RangedAttackWithSkillTwo(SkillManager.Instance.RangedAttack);
        }
    }

    /// <summary>
    /// When this strategy is performed, attack speed is updated with respect to the modifier.
    /// </summary>
    public class UpgradeAttackSpeed : ISkillStrategy
    {
        readonly Modifier _modifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modifier">The modifier to update the attack speed attribute.</param>
        public UpgradeAttackSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform()
        {
            SkillManager.Instance.RangedAttack.AttackSpeed.AddModifier(_modifier);
        }
    }

    /// <summary>
    /// When this strategy is performed, arrow speed is updated with respect to the modifier.
    /// </summary>
    public class UpgradeProjectileSpeed : ISkillStrategy
    {
        readonly Modifier _modifier;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modifier">The modifier to update the arrow speed attribute.</param>
        public UpgradeProjectileSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform()
        {
            SkillManager.Instance.RangedAttack.ProjectileSpeed.AddModifier(_modifier);
        }
    }

    /// <summary>
    /// When this strategy is performed, the fifth skill becomes active.
    /// </summary>
    public class CloneSkill : ISkillStrategy
    {
        public void Perform()
        {
            GameObject objectToBeCloned = SkillManager.Instance.gameObject;

            Vector3 randomPosition = new Vector3(Random.Range(-2f, 2f), Random.Range(-4f, 2f), -0.5f);

            GameObject clone = Object.Instantiate(objectToBeCloned, randomPosition, objectToBeCloned.transform.rotation);
            Object.Destroy(clone.GetComponent<SkillManager>());
            clone.name = "Cloned Character";
        }
    }
}