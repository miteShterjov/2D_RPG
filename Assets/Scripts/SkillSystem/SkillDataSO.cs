using System;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "Skill Data - ", menuName = "RPG Setup/Skill Data")]
    public class SkillDataSO : ScriptableObject
    {
        public int skillCost;
        public bool isUnlockedByDefault;
        public SkillType skillType;
        //public SkillUpgradeType upgradeType;
        public UpgradeData upgradeData;
        [Header( "Skill Info")]
        public string skillName;
        [TextArea]
        public string description;
        public Sprite icon;
    }
    
    [Serializable]
    public class UpgradeData
    {
        public SkillUpgradeType upgradeType;
        public float cooldown;
    }
}
