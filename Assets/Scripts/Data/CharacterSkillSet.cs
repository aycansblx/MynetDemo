using System.Collections.Generic;
using UnityEngine;

namespace MynetDemo.Data
{
    /// <summary>
    /// This class represents a skill set.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterSkillSet", menuName = "Character Skill Set", order = 2)]
    public class CharacterSkillSet : ScriptableObject
    {
        [SerializeField] List<CharacterSkill> _skills;

        /// <summary>
        /// Returns the skills in the set.
        /// </summary>
        public List<CharacterSkill> Skills { get { return _skills; } }

        /// <summary>
        /// Inserts a skill into the given index.
        /// This function is not used in the demo, but it should exist to give opinion about this class.
        /// </summary>
        /// <param name="skill">Skill to be added.</param>
        /// <param name="index">Index of the skill in the list</param>
        public void AddSkill(CharacterSkill skill, int index)
        {
            Skills.Insert(index, skill);
        }

        /// <summary>
        /// Removes a skill from the given index.
        /// </summary>
        /// <param name="index">The index of the skill to be removed.</param>
        public void RemoveSkill (int index)
        {
            Skills.RemoveAt(index);
        }
    }
}
