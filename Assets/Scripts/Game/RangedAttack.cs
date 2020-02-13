using UnityEngine;
using MynetDemo.Core;
using MynetDemo.Manager;

namespace MynetDemo.Game
{
    public interface IRangedAttack
    {
        bool UpdateTimer(float deltaTime);
        void Shoot(Vector3 position, float direction);
    }

    public class DefaultRangedAttack : IRangedAttack
    {
        float _timer;

        readonly GameObject _projectile;

        public Attribute AttackSpeed { get; private set; }
        public Attribute ArrowSpeed { get; private set; }

        public DefaultRangedAttack(GameObject projectile,  float attackSpeed, float arrowSpeed)
        {
            _projectile = projectile;

            AttackSpeed = new Attribute(attackSpeed);
            ArrowSpeed = new Attribute(arrowSpeed);
        }

        public DefaultRangedAttack(DefaultRangedAttack rangedAttack)
        {
            _timer = rangedAttack._timer;
            _projectile = rangedAttack._projectile;

            AttackSpeed = rangedAttack.AttackSpeed;
            ArrowSpeed = rangedAttack.ArrowSpeed;
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
            PoolingManager.Instance.Get(_projectile, position, Quaternion.Euler(0f, 0f, direction + 90f));
        }
    }

    public abstract class RangedAttackDecorator : IRangedAttack
    {
        protected IRangedAttack _rangedAttack;

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

    public class RangedAttackWithSkillTwo:  RangedAttackDecorator
    {
        const float _DURATION_BETWEEN_ATTACKS_ = 0.3f;

        public RangedAttackWithSkillTwo(IRangedAttack rangedAttack) : base(rangedAttack) { }

        public override bool UpdateTimer(float deltaTime)
        {
            return base.UpdateTimer(deltaTime);
        }

        public override void Shoot(Vector3 position, float direction)
        {
            base.Shoot(position, direction);
        }
    }
}