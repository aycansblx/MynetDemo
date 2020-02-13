using MynetDemo.Core;

namespace MynetDemo.Game
{
    public interface ISkillStrategy
    {
        void Perform(Character character);
    }

    public class CharacterSkill
    {
        public ISkillStrategy Strategy { get; private set; }

        public CharacterSkill(ISkillStrategy strategy)
        {
            Strategy = strategy;
        }

        public void Perform(Character character)
        {
            Strategy.Perform(character);
        }
    }

    public class UpgradeToSkillOne : ISkillStrategy
    {
        public void Perform(Character character)
        {
            character.RangedAttack = new RangedAttackWithSkillOne(character.RangedAttack);
        }
    }

    public class UpgradeToSkillTwo : ISkillStrategy
    {
        public void Perform(Character character)
        {
            character.RangedAttack = new RangedAttackWithSkillTwo(character.RangedAttack);
        }
    }

    public class UpgradeAttackSpeed : ISkillStrategy
    {
        Modifier _modifier;

        public UpgradeAttackSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform(Character character)
        {
            character.RangedAttack.AttackSpeed.AddModifier(_modifier);
        }
    }

    public class UpgradeProjectileSpeed : ISkillStrategy
    {
        Modifier _modifier;

        public UpgradeProjectileSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform(Character character)
        {
            character.RangedAttack.ProjectileSpeed.AddModifier(_modifier);
        }
    }

    public class CloneSkill : ISkillStrategy
    {
        public void Perform(Character character)
        {
            // LOL
        }
    }
}