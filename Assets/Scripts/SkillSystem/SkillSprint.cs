using System.Collections;
using UnityEngine;

namespace SkillSystem
{
    public class SkillSprint : SkillBase
    {
        [Header("Sprint Cooldown")]
        [SerializeField, Min(1)] private int maxSprintCharges = 2;
        [SerializeField, Min(0f)] private float defaultSprintCooldown = 2f;

        private int currentSprintCharges;

        protected override void Awake()
        {
            base.Awake();
            currentSprintCharges = maxSprintCharges;
        }

        public bool CanUseSprint() => upgradeType != SkillUpgradeType.None && currentSprintCharges > 0;

        public bool TryConsumeSprintCharge()
        {
            if (!CanUseSprint()) return false;

            currentSprintCharges--;
            if (currentSprintCharges == 0) StartCoroutine(RefreshSprintCharges());
            return true;
        }

        public void OnStartEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStartAndArrival))
            {
                CreateClone();
            }
            
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStartAndArrival))
            {
                CreateShard();
            }
        }

        public void OnEndEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStartAndArrival)) CreateClone();
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStartAndArrival)) CreateShard();
        }
        
        private void CreateShard()
        {
            skillsManager.Shard.CreateRawShard();
        }

        private static void CreateClone()
        {
            print("Creating time echo");
            //todo: create time echo logic
        }

        private IEnumerator RefreshSprintCharges()
        {
            float rechargeDuration = Mathf.Max(defaultSprintCooldown, cooldown);
            yield return new WaitForSeconds(rechargeDuration);
            currentSprintCharges = maxSprintCharges;
        }
    }
}