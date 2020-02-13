using UnityEngine;

namespace MynetDemo.Data
{
    /// <summary>
    /// An object of this class represents a skill.
    /// When adding a skill, follow these steps;
    ///     1) Add a code to SkillCode list.
    ///     2) Create a Character Skill asset and fill the inspector window attributes.
    ///     3) Insert the related logic to SkillManager class.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterSkill", menuName = "Character Skill", order = 1)]
    public class CharacterSkill : ScriptableObject
    {
        [SerializeField] string _name;

        [SerializeField] string _description;

        [SerializeField] Sprite _image;

        [SerializeField] SkillCode _code;

        /// <summary>
        /// Returns the skill name.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Returns the skill description.
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// Returns the skill image.
        /// </summary>
        public Sprite Image { get { return _image; } }

        public delegate void OnSkillEvent(SkillCode code);
        public static OnSkillEvent OnSkill;

        /// <summary>
        /// Call this function to activate the class.
        /// </summary>
        public void Activate() { OnSkill?.Invoke(_code); }
    }

    public enum SkillCode
    {
        TripleShot, SequentialShot, DecreaseCooldown, IncreaseArrowSpeed, Clone,
    }
}
