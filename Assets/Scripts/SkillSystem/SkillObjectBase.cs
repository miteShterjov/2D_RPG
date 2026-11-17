using System;
using Data.ElementalEffectData;
using EntityControl;
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
        [SerializeField] protected float nearestTargetCheck = 10f;

        protected GeneralStats entityStats;
        protected DamageScaleData damageScaleData;
        protected ElementType usedElement;
        
        private Collider2D[] EnemiesAround(Transform t, float radius) =>
            Physics2D.OverlapCircleAll(t.position, radius, enemyMask);

        protected void DamageEnemiesInRange(Transform t, float radius)
        {
            foreach (Collider2D target in EnemiesAround(t, radius))
            {
                IDamageable damageable = target.GetComponent<IDamageable>();
                if (damageable == null) continue;
                
                AttackData attackData = entityStats.GetAttackData(damageScaleData);
                EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

                float physicalDamage = attackData.physicalDamage;
                float elementalDamage = attackData.elementalDamage;
                ElementType element = attackData.elementalType;
                
                damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);
                
                if (element != ElementType.None) statusHandler?.ApplyStatusEffect(element, attackData.elementalEffectData);
                
                usedElement = element;
            }
        }

        protected Transform FindNearestTarget()
        {
            Transform target = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D enemy in EnemiesAround(transform, nearestTargetCheck))
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