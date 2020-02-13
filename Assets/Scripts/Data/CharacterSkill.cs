using UnityEngine;

namespace MynetDemo.Data
{
    [CreateAssetMenu(fileName = "NewCharacterSkill", menuName = "Character Skill", order = 1)]

    public class CharacterSkill : ScriptableObject
    {
        [SerializeField] string _name;

        [SerializeField] Sprite _sprite;

        [SerializeField] SkillCode _code;

        public delegate void OnSkillEvent(SkillCode code);
        public static OnSkillEvent OnSkill;

        public string GetName() { return _name; }

        public Sprite GetSprite() { return _sprite; }

        public void Activate() { OnSkill?.Invoke(_code); Debug.Log(_name); }
    }

    public enum SkillCode
    {
        TripleShot, SequentialShot, DecreaseCooldown, IncreaseArrowSpeed, Clone,
    }
}
