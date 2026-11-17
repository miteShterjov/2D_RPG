using System.Collections;
using System.Collections.Generic;
using Data.ElementalEffectData;
using EntityStats;
using Interface;
using UnityEngine;

namespace EntityControl
{
    public class EntityCombatController : MonoBehaviour
    {
        public Collider2D[] targetColliders;
        public DamageScaleData basicAttackScale;
        
        [Header("Target detection")]
        [SerializeField] private Transform targetCheck;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private LayerMask targetMask;
        [Header("Status Effects Config")]
        [SerializeField] private float statusDefaultDuration = 10f;
        [SerializeField] private float slowSpeedMultiplier = .2f;
        [SerializeField] private float staticChargeBuildUp = .4f;
        [Space] 
        [SerializeField] private float fireScale = .8f;
        [SerializeField] private float lightningScale = 2.5f;

        private EntityVFX entityVFX;
        private GeneralStats generalStats;
        
        private Coroutine slowDownCoroutine;

        protected virtual void Awake()
        {
            entityVFX = GetComponent<EntityVFX>();
            generalStats = GetComponent<GeneralStats>();
        }

        public virtual void PreformAttack()
        {
            GetDetectedColliders();
            HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

            foreach (Collider2D target in targetColliders)
            {
                IDamageable damageable = target.GetComponentInParent<IDamageable>();
                AttackData attackData = generalStats.GetAttackData(basicAttackScale);
                EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();
                
                if (damageable == null || !hitTargets.Add(damageable)) continue;
                
                float physDmg = attackData.physicalDamage;
                float eleDmg = attackData.elementalDamage;
                ElementType element = attackData.elementalType;
                
                bool wasTargetHit = damageable.TakeDamage(physDmg,eleDmg, element, transform);
                
                if (!wasTargetHit) continue;
                if (element != ElementType.None) statusHandler?.ApplyStatusEffect(element, attackData.elementalEffectData);
                
                entityVFX?.UpdateOnHitEffectColor(element);
                entityVFX?.PlayOnHitEffect(target.transform, attackData.isCritical);
            }
        }
        
        public virtual void SlowDownEntityBy(float duration, float slowMultiplier)
        {
            if (slowDownCoroutine != null) StopCoroutine(slowDownCoroutine);
            slowDownCoroutine = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
        }

        protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
        {
            yield return null;
        }
        
        protected void GetDetectedColliders()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            targetColliders = Physics2D.OverlapCircleAll(
                targetCheck.position, 
                targetCheckRadius, 
                targetMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        }
    }
}