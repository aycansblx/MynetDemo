using System.Collections.Generic;
using UnityEngine;

namespace MynetDemo.Data
{
    [CreateAssetMenu(fileName = "NewCharacterSkillSet", menuName = "Character Skill Set", order = 1)]
    public class CharacterSkillSet : ScriptableObject
    {
        [SerializeField] List<CharacterSkill> _skills;

        public List<CharacterSkill> Skills { get { return _skills; } }
    }
}
