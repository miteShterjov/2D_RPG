using System.Collections.Generic;
using Data.ElementalEffectData;
using EntityControl;
using EntityStats;
using Interface;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectBase : MonoBehaviour
    {
        [Header("Skills Settings")] 
        [SerializeField] protected GameObject onHitEffectPrefab;
        [SerializeField] protected LayerMask enemyMask;
        [SerializeField] protected Transform targetCheck;
        [SerializeField] protected float checkRadius = 1f;
        [SerializeField] protected float nearestTargetCheck = 10f;

        protected GeneralStats entityStats;
        protected DamageScaleData damageScaleData;
        protected ElementType usedElement;
        protected bool targetGotHit;
        protected Transform lastTarget;
        protected Animator animator;
        protected Rigidbody2D rb;
        
        private readonly List<Collider2D> enemyBuffer = new();
        private ContactFilter2D enemyFilter;

        protected virtual bool RequiresAnimator => false;

        protected virtual void Awake()
        {
            enemyFilter = new ContactFilter2D();
            enemyFilter.SetLayerMask(enemyMask);
            enemyFilter.useTriggers = true;
            animator = GetComponentInChildren<Animator>();
            if (animator == null && RequiresAnimator)
                Debug.LogError($"Animator not found on {GetType().Name}", this);
            rb = GetComponent<Rigidbody2D>();
        }
        
        protected List<Collider2D> GetEnemiesAround(Transform t, float radius)
        {
            enemyBuffer.Clear();
            Physics2D.OverlapCircle(t.position, radius, enemyFilter, enemyBuffer);
            return enemyBuffer;
        }
        
        protected void DamageEnemiesInRange(Transform t, float radius)
        {
            foreach (Collider2D target in GetEnemiesAround(t, radius))
            {
                IDamageable damageable = target.GetComponent<IDamageable>();
                if (damageable == null) continue;
                
                AttackData attackData = entityStats.GetAttackData(damageScaleData);
                EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

                float physicalDamage = attackData.physicalDamage;
                float elementalDamage = attackData.elementalDamage;
                ElementType element = attackData.elementalType;
                
                targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);
                
                if (element != ElementType.None) statusHandler?.ApplyStatusEffect(element, attackData.elementalEffectData);
                if (targetGotHit)
                {
                    lastTarget = target.transform;
                    Instantiate(onHitEffectPrefab, target.transform.position, Quaternion.identity);
                }
                
                usedElement = element;
            }
        }

        protected Transform FindNearestTarget()
        {
            Transform target = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D enemy in GetEnemiesAround(transform, nearestTargetCheck))
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