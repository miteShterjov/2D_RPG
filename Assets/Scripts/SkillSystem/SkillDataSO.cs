using System;
using Data.ElementalEffectData;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "Skill Data - ", menuName = "RPG Setup/Skill Data")]
    public class SkillDataSO : ScriptableObject
    {
        [Header("Skill Info")]
        public string skillName;
        [TextArea]
        public string description;
        public Sprite icon;
        
        [Header("Unlock & Upgrade")]
        public int skillCost;
        public bool isUnlockedByDefault;
        public SkillType skillType;
        public UpgradeData upgradeData;
        
        
    }
    
    [Serializable]
    public class UpgradeData
    {
        public SkillUpgradeType upgradeType;
        public float cooldown;
        public DamageScaleData damageScaleData;
    }
}
