using Interface;
using Misc;
using UnityEngine;

namespace EntityControl
{
    public class EntityCombatController : MonoBehaviour
    {
        public Collider2D[] targetColliders; 
        
        [Header("Target detection")]
        [SerializeField] private Transform targetCheck;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float damage = 10f;

        private EntityVFX entityVFX;

        protected virtual void Awake()
        {
            entityVFX = GetComponent<EntityVFX>();
        }


        public virtual void PreformAttackEffect()
        {
            GetDetectedColliders();

            foreach (Collider2D target in targetColliders)
            {
                IDamageable damageable = target.GetComponent<IDamageable>();
                
                if (damageable == null) continue;
                
                damageable.TakeDamage(damage, transform);
                entityVFX.PlayOnHitEffect(target.transform);
            }
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