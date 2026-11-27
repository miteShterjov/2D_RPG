using Data.ElementalEffectData;
using EntityControl;
using PlayerControl;
using Unity.VisualScripting;
using UnityEngine;

namespace SkillSystem
{
    public class SkillBase : MonoBehaviour
    {
        [DoNotSerialize]
        [HideInInspector] public PlayerController player;
        [HideInInspector] public EntityHealthController entityHealthController;
        [HideInInspector] public DamageScaleData damageScaleData;
        [HideInInspector] public PlayerSkillsManager skillsManager;
        
        [Header("General Details")]
        [SerializeField] protected SkillType skillType;
        [SerializeField] protected SkillUpgradeType upgradeType;
        [SerializeField] protected float cooldown;
        
        private float lastTimeUsed;

        protected virtual void Awake()
        {
            player = GetComponentInParent<PlayerController>();
            if (player == null) Debug.LogError("PlayerController player is null in SkillBase.cs!");
            skillsManager = player.GetComponent<PlayerSkillsManager>();
            entityHealthController = player.GetComponent<EntityHealthController>();
            lastTimeUsed -= cooldown;
        }

        public virtual void TryUseSkill()
        {
        }

        public void SetSkillUpgrade(UpgradeData upgradeData)
        {
            upgradeType = upgradeData.upgradeType;
            cooldown = upgradeData.cooldown;
            damageScaleData = upgradeData.damageScaleData ?? damageScaleData ?? new DamageScaleData();
        }
        
        public virtual bool CanUseSkill()
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