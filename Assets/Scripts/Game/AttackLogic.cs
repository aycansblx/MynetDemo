using DG.Tweening;
using MynetDemo.Core;
using MynetDemo.Manager;
using UnityEngine;

namespace MynetDemo.Game
{
    /// <summary>
    /// Ranged attack interface.
    /// </summary>
    public interface IRangedAttack
    {
        /// <summary>
        /// Returns the attack speed attribute.
        /// </summary>
        Attribute AttackSpeed { get; }

        /// <summary>
        /// Returns the projectile speed attribute.
        /// </summary>
        Attribute ProjectileSpeed { get; }

        /// <summary>
        /// Updates the ranged attack timer.
        /// </summary>
        /// <param name="deltaTime">Passed time since the previous frame.</param>
        /// <returns>True if the timer exceeds AttackSpeed attribute value.</returns>
        bool UpdateTimer(float deltaTime);

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="position">Starting position of the projectile.</param>
        /// <param name="direction">Movement direction of the projectile.</param>
        void Shoot(Vector3 position, float direction);
    }

    /// <summary> 
    /// The default ranged attack logic. It is a concrete class.
    /// </summary>
    public class DefaultRangedAttack : IRangedAttack
    {
        const float _PROJECTILE_RANGE_ = 15f;

        float _timer;

        readonly GameObject _projectile;

        public Attribute AttackSpeed { get; }
        public Attribute ProjectileSpeed { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="projectile">Projectile prefab.</param>
        /// <param name="attackSpeed">Base attack speed.</param>
        /// <param name="projectileSpeed">Base projectile speed.</param>
        public DefaultRangedAttack(GameObject projectile,  float attackSpeed, float projectileSpeed)
        {
            _projectile = projectile;

            AttackSpeed = new Attribute(attackSpeed);
            ProjectileSpeed = new Attribute(projectileSpeed);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="rangedAttack">The ranged attack logic which will be copied.</param>
        public DefaultRangedAttack(DefaultRangedAttack rangedAttack)
        {
            _timer = rangedAttack._timer;
            _projectile = rangedAttack._projectile;

            AttackSpeed = rangedAttack.AttackSpeed;
            ProjectileSpeed = rangedAttack.ProjectileSpeed;
        }

        public bool UpdateTimer(float deltaTime)
        {
            if ((_timer += deltaTime) > AttackSpeed.Value)
            {
                _timer = 0f;
                return true;
            }
            return false;
        }

        public void Shoot(Vector3 position, float direction)
        {
            GameObject projectile = PoolingManager.Instance.Get(_projectile, position, Quaternion.Euler(0f, 0f, direction + 90f));

            Vector3 target = projectile.transform.position;

            target.x += _PROJECTILE_RANGE_ * Mathf.Cos(direction * Mathf.Deg2Rad);
            target.y += _PROJECTILE_RANGE_ * Mathf.Sin(direction * Mathf.Deg2Rad);

            projectile.transform.DOMove(target, _PROJECTILE_RANGE_ / ProjectileSpeed.Value).OnComplete(() => {
                PoolingManager.Instance.Add(projectile);
            });
        }
    }

    /// <summary>
    /// The decorator for the ranged attack logic.
    /// </summary>
    public abstract class RangedAttackDecorator : IRangedAttack
    {
        protected IRangedAttack _rangedAttack;

        public Attribute AttackSpeed { get { return _rangedAttack.AttackSpeed; } }
        public Attribute ProjectileSpeed { get { return _rangedAttack.ProjectileSpeed; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rangedAttack">The ranged attack logic which will be decorated.</param>
        public RangedAttackDecorator(IRangedAttack rangedAttack)
        {
            _rangedAttack = rangedAttack;
        }

        public virtual bool UpdateTimer(float deltaTime)
        {
            return _rangedAttack.UpdateTimer(deltaTime);
        }

        public virtual void Shoot(Vector3 position, float direction)
        {
            _rangedAttack.Shoot(position, direction);
        }
    }

    /// <summary>
    /// Shoots triple arrows.
    /// </summary>
    public class RangedAttackWithSkillOne : RangedAttackDecorator
    {
        public RangedAttackWithSkillOne(IRangedAttack rangedAttack) : base(rangedAttack) { }

        public override void Shoot(Vector3 position, float direction)
        {
            base.Shoot(position, direction - 45f);
            base.Shoot(position, direction);
            base.Shoot(position, direction + 45f);
        }
    }

    /// <summary>
    /// Shoots two sequential arrows.
    /// </summary>
    public class RangedAttackWithSkillTwo:  RangedAttackDecorator
    {
        const float _DURATION_BETWEEN_ATTACKS_ = 0.3f;

        float _secondAttackTimer;

        public RangedAttackWithSkillTwo(IRangedAttack rangedAttack) : base(rangedAttack)
        {
            _secondAttackTimer = 0f;
        }

        public override bool UpdateTimer(float deltaTime)
        {
            if (_secondAttackTimer == 0f)
            {
                if (base.UpdateTimer(deltaTime))
                {
                    _secondAttackTimer = _DURATION_BETWEEN_ATTACKS_;
                    return true;
                }
                return false;
            }
            else
            {
                _secondAttackTimer -= deltaTime;
                if (_secondAttackTimer <= 0f)
                {
                    _secondAttackTimer = 0f;
                    return true;
                }
                return false;
            }
        }

        public override void Shoot(Vector3 position, float direction)
        {
            base.Shoot(position, direction);
        }
    }
}