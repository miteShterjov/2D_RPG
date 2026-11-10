using System.Collections;
using EntityControl;
using UnityEngine;

namespace SkillSystem
{
    public class SkillShard : SkillBase
    {
        [Header("Shard Details")]
        [SerializeField] private GameObject shardPrefab;
        [SerializeField] private float detonationTime = 5f;
        [SerializeField] private float shardSpeed = 7f;

        [Header("Shard Multicast upgrade")] 
        [SerializeField] private int maxCharges = 3;
        [SerializeField] private int currentCharge;
        [SerializeField] private bool isCharging;

        [Header("Shard teleport upgrade")] 
        [SerializeField] private float shardExistDuration = 10f;

        [Header("Health Rewind upgrade")] 
        [SerializeField] private float savedHealthPercent;

        private SkillObjectShard currentShard;

        public override void TryUseSkill()
        {
            if (!CanUseSkill()) return;
            if (UpgradeIsUnlocked(SkillUpgradeType.Shard)) HandleShardRegular();
            if (UpgradeIsUnlocked(SkillUpgradeType.ShardMoveToEnemy)) HandleShardMovement();
            if (UpgradeIsUnlocked(SkillUpgradeType.ShardMultiCast)) HandleShardMultiCast();
            if (UpgradeIsUnlocked(SkillUpgradeType.ShardTeleport)) HandleShardTeleport();
            if (UpgradeIsUnlocked(SkillUpgradeType.ShardTeleportAndHpRewind)) HandleShardHealthRewind();
        }

        private float GetDetonationTime() => UpgradeIsUnlocked(SkillUpgradeType.ShardTeleport) ? shardExistDuration : detonationTime;

        private void ForceCooldown()
        {
            if (OnCooldown()) return;
            
            SetSkillOnCooldown();
            currentShard.OnShardExplode -= ForceCooldown;
        }
        
        private void HandleShardRegular()
        {
            CreateShard();
            SetSkillOnCooldown();
        }

        private void CreateShard()
        {
            float boomTime = GetDetonationTime();
            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
            currentShard = shard.GetComponent<SkillObjectShard>();
            currentShard.SetupShard(boomTime);

            if (UpgradeIsUnlocked(SkillUpgradeType.ShardTeleport)
                || UpgradeIsUnlocked(SkillUpgradeType.ShardTeleportAndHpRewind))
            {
                currentShard.OnShardExplode += ForceCooldown;
            }
        }

        private void HandleShardMovement()
        {
            CreateShard();
            currentShard.MoveTowardsNearestTarget(shardSpeed);
            SetSkillOnCooldown();
        }

        private void HandleShardMultiCast()
        {
            if (isCharging) return;
            if (currentCharge <= 0) return;

            currentCharge--;
            CreateShard();
            currentShard.MoveTowardsNearestTarget(shardSpeed);
            
            if (!isCharging) StartCoroutine(ShardRechargeCo());
        }

        private void HandleShardTeleport()
        {
            if (!currentShard) CreateShard();
            else
            {
                SwapPlayerAndShard();
                SetSkillOnCooldown();   
            }
        }

        private void HandleShardHealthRewind()
        {
            if (!currentShard)
            {
                CreateShard();
                savedHealthPercent = entityHealthController.GetHealthPercentage();
            }
            else
            {
                SwapPlayerAndShard();
                entityHealthController.SetHealthToPercentage(savedHealthPercent);
                SetSkillOnCooldown();   
            }
        }

        private void SwapPlayerAndShard()
        {
            Vector3 shardPosition = currentShard.transform.position;
            Vector3 playerPosition = player.transform.position; 
            
            currentShard.transform.position = playerPosition;
            currentShard.ShardExplode();
            player.playerMove.TeleportPlayer(shardPosition);
        }
        
        private IEnumerator ShardRechargeCo()
        {
            isCharging = true;

            while (currentCharge < maxCharges)
            {
                yield return new WaitForSeconds(cooldown);
                currentCharge++;
            }
            
            isCharging = false;
        }
        
    }
}