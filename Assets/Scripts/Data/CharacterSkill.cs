using UnityEngine;

namespace MynetDemo.Data
{
    /// <summary>
    /// An object of this class represents a skill.
    /// 
    /// When adding a skill, follow these steps;
    ///     1) Add a code to SkillCode list below.
    ///     2) Create a CharacterSkill asset and fill the inspector window attributes.
    ///     3) Insert the related logic to SkillManager class.
    ///         3A) If your skill contains a completely new logic, implement
    ///             necessary strategy in SkillLogic collection.
    ///             
    /// When you add the skill asset to the active character skill set asset
    /// it is rendered in the ui and becomes castable.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterSkill", menuName = "Character Skill", order = 1)]
    public class CharacterSkill : ScriptableObject
    {
        [SerializeField] SkillCode _code;

        [SerializeField] string _name;

        [SerializeField] Sprite _image;

        [SerializeField] [TextArea] string _description;

        /// <summary>
        /// Returns the skill code.
        /// </summary>
        public SkillCode Code { get { return _code; } }

        /// <summary>
        /// Returns the skill name.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Returns the skill image.
        /// </summary>
        public Sprite Image { get { return _image; } }

        /// <summary>
        /// Returns the skill description.
        /// </summary>
        public string Description { get { return _description; } }
    }

    public enum SkillCode
    {
        TripleShot, DoubleShot, Haste, UberBolt, Clone,
    }
}
