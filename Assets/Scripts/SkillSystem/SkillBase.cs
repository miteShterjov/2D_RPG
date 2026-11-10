using EntityControl;
using PlayerControl;
using UnityEngine;

namespace SkillSystem
{
    public class SkillBase : MonoBehaviour
    {
        public PlayerController player;
        public EntityHealthController entityHealthController;
        
        [Header("General Details")]
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SkillUpgradeType upgradeType;
        [SerializeField] protected float cooldown;
        
        private float lastTimeUsed;

        protected virtual void Awake()
        {
            player = GetComponentInParent<PlayerController>();
            if (!player) Debug.LogError("PlayerController player is null in SkillBase.cs!");
            lastTimeUsed -= cooldown;
        }

        public virtual void TryUseSkill()
        {
        }

        public void SetSkillUpgrade(UpgradeData upgradeData)
        {
            upgradeType = upgradeData.upgradeType;
            cooldown = upgradeData.cooldown;
        }
        
        public bool CanUseSkill()
        {
            if (upgradeType == SkillUpgradeType.None) return false;
            return !OnCooldown();
        }
        
        public void ResetCooldown() => lastTimeUsed = Time.time;
        
        public void ResetCooldownBy(float reduction) => lastTimeUsed -= reduction;
        
        public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
        
        protected bool UpgradeIsUnlocked(SkillUpgradeType skillUpgradeType) => this.upgradeType == skillUpgradeType;
        
        protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    }
}