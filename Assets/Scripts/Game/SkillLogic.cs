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
    public class UpgradeToTripleShot : ISkillStrategy
    {
        public void Perform()
        {
            SkillManager.Instance.RangedAttack = new TripleShotAttack(SkillManager.Instance.RangedAttack);
        }
    }

    /// <summary>
    /// When this strategy is performed, the second skill becomes active.
    /// </summary>
    public class UpgradeToDoubleShot : ISkillStrategy
    {
        public void Perform()
        {
            SkillManager.Instance.RangedAttack = new DoubleShotAttack(SkillManager.Instance.RangedAttack);
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
    /// When this strategy is performed, the character is cloned in a random position.
    /// </summary>
    public class CloneTheCharacterRandomly : ISkillStrategy
    {
        const float _Z_ = -.5f;

        /* Scene Extents... */
        readonly Vector2 _MIN_ = new Vector2(-2f, -4f);
        readonly Vector2 _MAX_ = new Vector2(2f, 2f);

        public void Perform()
        {
            GameObject objectToBeCloned = SkillManager.Instance.gameObject;

            Vector3 rndpos = new Vector3(Random.Range(_MIN_.x, _MAX_.x), Random.Range(_MIN_.y, _MAX_.y), _Z_);

            GameObject clone = PoolingManager.Instance.Get(objectToBeCloned, rndpos, objectToBeCloned.transform.rotation);

            Object.Destroy(clone.GetComponent<SkillManager>());
        }
    }
}