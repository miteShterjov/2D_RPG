using System;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectShard : SkillObjectBase
    {
        public event Action OnShardExplode;
        [SerializeField] private GameObject effectPrefab;

        private Transform target;
        private float speed;

        private SkillShard shardHandler;

        private void Update()
        {
            if (target == null) return;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        public void MoveTowardsNearestTarget(float shardSpeed)
        {
            speed = shardSpeed;
            target = FindNearestTarget();
        }

        public void SetupShard(SkillShard skillShard)
        {
            this.shardHandler = skillShard;
            float detonationTime = shardHandler.GetDetonationTime();
            entityStats = shardHandler.player.generalStats;
            damageScaleData = shardHandler.damageScaleData;
            Invoke(nameof(ShardExplode), detonationTime);
        }

        public void SetupShard(SkillShard shardManager, float detonationTime, bool canMove, float shardMoveSpeed)
        {
            this.shardHandler = shardManager;
            entityStats = shardHandler.player.generalStats;
            damageScaleData = shardHandler.damageScaleData;
            
            Invoke(nameof(ShardExplode), detonationTime);
            if (canMove) MoveTowardsNearestTarget(shardMoveSpeed);
        }
        
        public void ShardExplode()
        {
            DamageEnemiesInRange(transform, checkRadius);
            GameObject visualEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            visualEffect.GetComponentInChildren<SpriteRenderer>().color = shardHandler.player.playerVFX.UpdateOnHitEffectColor(usedElement);
            OnShardExplode?.Invoke();
            Destroy(gameObject);
        }
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;

            ShardExplode();
        }
    }
}