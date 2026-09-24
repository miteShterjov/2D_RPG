using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "Skill Data - ", menuName = "RPG Setup/Skill Data")]
    public class SkillDataSO : ScriptableObject
    {
        public int skillCost;
        [Header( "Skill Info")]
        public string skillName;
        [TextArea]
        public string description;
        public Sprite icon;
    }
}
