using System.Collections;
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

        public void CreateRawShard()
        {
            bool canMove = UpgradeIsUnlocked(SkillUpgradeType.ShardMoveToEnemy) || UpgradeIsUnlocked(SkillUpgradeType.ShardMultiCast);
            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
            shard.GetComponent<SkillObjectShard>().SetupShard(this, GetDetonationTime(), canMove, shardSpeed);
        }

        public float GetDetonationTime() => UpgradeIsUnlocked(SkillUpgradeType.ShardTeleport) ? shardExistDuration : detonationTime;

        private void ForceCooldown()
        {
            if (!OnCooldown()) SetSkillOnCooldown();

            SkillObjectShard explodedShard = currentShard;
            currentShard = null;
            if (explodedShard) explodedShard.OnShardExplode -= ForceCooldown;
        }
        
        private void HandleShardRegular()
        {
            CreateShard();
            SetSkillOnCooldown();
        }

        private void CreateShard()
        {
            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
            currentShard = shard.GetComponent<SkillObjectShard>();
            currentShard.SetupShard(this);

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
            if (currentShard == null) CreateShard();
            else
            {
                SwapPlayerAndShard();
                SetSkillOnCooldown();   
            }
        }

        private void HandleShardHealthRewind()
        {
            if (currentShard == null)
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