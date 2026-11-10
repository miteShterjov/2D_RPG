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

        public void SetupShard(float detonationTime)
        {
            Invoke(nameof(ShardExplode), detonationTime);
        }
        
        public void ShardExplode()
        {
            DamageEnemiesInRange(transform, checkRadius);
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            OnShardExplode?.Invoke();
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;

            ShardExplode();
        }
    }
}