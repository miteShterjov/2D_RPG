using UnityEngine;

namespace SkillSystem
{
    public class SkillBase : MonoBehaviour
    {
        [Header("General Details")]
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SkillUpgradeType upgradeType;
        [SerializeField] private float cooldown;
        
        private float lastTimeUsed;

        protected virtual void Awake()
        {
            lastTimeUsed -= cooldown;
        }
        
        protected bool UpgradeIsUnlocked(SkillUpgradeType skillUpgradeType) => this.upgradeType == skillUpgradeType; 
        
        public void SetSkillUpgrade(UpgradeData upgradeData)
        {
            upgradeType = upgradeData.upgradeType;
            cooldown = upgradeData.cooldown;
        }
        
        public bool CanUseSkill() => !OnCooldown;
        
        public void ResetCooldown() => lastTimeUsed = Time.time;
        public void ResetCooldownBy(float reduction) => lastTimeUsed -= reduction;
        
        public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
        private bool OnCooldown => Time.time < lastTimeUsed + cooldown;
    }
}