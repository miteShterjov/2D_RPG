using System;
using EntityStats;
using Interface;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectBase : MonoBehaviour
    {
        [SerializeField] protected LayerMask enemyMask;
        [SerializeField] protected Transform targetCheck;
        [SerializeField] protected float checkRadius = 1f;

        protected Collider2D[] EnemiesAround(Transform t, float radius) =>
            Physics2D.OverlapCircleAll(t.position, radius, enemyMask);

        protected void DamageEnemiesInRange(Transform t, float radius)
        {
            foreach (Collider2D target in EnemiesAround(t, radius))
            {
                IDamageable damageable = target.GetComponent<IDamageable>();
                if (damageable == null) continue;
                damageable.TakeDamage(1, 1, ElementType.None, transform);
            }
        }

        protected Transform FindNearestTarget()
        {
            Transform target = null;
            float closestDistance = Mathf.Infinity;

            foreach (var enemy in EnemiesAround(transform, 10))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (!(distance < closestDistance)) continue;
                target = enemy.transform;
                closestDistance = distance;
            }

            return target;
        }

        protected void OnDrawGizmos()
        {
            if (targetCheck == null) targetCheck = transform;
            Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
        }
    }
}