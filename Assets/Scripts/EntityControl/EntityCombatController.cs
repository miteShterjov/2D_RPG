using System.Collections;
using System.Collections.Generic;
using EntityStats;
using Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityControl
{
    public class EntityCombatController : MonoBehaviour
    {
        public Collider2D[] targetColliders; 
        
        [Header("Target detection")]
        [SerializeField] private Transform targetCheck;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private LayerMask targetMask;
        [FormerlySerializedAs("statusEffectDuration")]
        [Header("Status Effects Config")]
        [SerializeField] private float statusDefaultDuration = 10f;
        [SerializeField] private float slowSpeedMultiplier = .2f;
        [SerializeField] private float staticChargeBuildUp = .4f;
        [Space] [SerializeField] private float fireScale = .8f;
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
                
                if (damageable == null || !hitTargets.Add(damageable)) continue;
                float eleDmg = generalStats.GetElementalDamage(out ElementType element);
                bool wasTargetHit = damageable.TakeDamage(
                    generalStats.GetPhysicalDamage(out bool isCritAttack), 
                    eleDmg, 
                    element, 
                    transform);
                if (!wasTargetHit) continue;
                if (element != ElementType.None) ApplyStatusEffect(target.transform, element);
                entityVFX?.UpdateOnHitEffectColor(element);
                entityVFX?.PlayOnHitEffect(target.transform, isCritAttack);
            }
        }
        
        public virtual void SlowDownEntityBy(float duration, float slowMultiplier)
        {
            if (slowDownCoroutine != null) StopCoroutine(slowDownCoroutine);
            slowDownCoroutine = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
        }

        private void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
        {
            EntityStatusHandler statusHandler = target.GetComponentInParent<EntityStatusHandler>();
            
            if (!statusHandler) return;

            switch (element)
            {
                case ElementType.Ice when statusHandler.CanEffectBeApplied(ElementType.Ice):
                    statusHandler.ApplyChillEffect(statusDefaultDuration, slowSpeedMultiplier);
                    break;
                case ElementType.Fire when statusHandler.CanEffectBeApplied(ElementType.Fire):
                    scaleFactor = fireScale;
                    statusHandler.ApplyBurningEffect(statusDefaultDuration, generalStats.GetElementalDamage());
                    break;
                case ElementType.Lightning when statusHandler.CanEffectBeApplied(ElementType.Lightning):
                    scaleFactor = lightningScale;
                    statusHandler.ApplyLightningEffect(statusDefaultDuration, generalStats.GetElementalDamage(), staticChargeBuildUp);
                    break;
                case ElementType.None:
                    break;
                default:
                    statusHandler.StopAllStatusEffects();
                    break;
            }
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