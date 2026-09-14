using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace EntityControl
{
    public class EntityHealthController : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float currentHp;
        [SerializeField] public bool isDead;
        
        private EntityStats.EntityStats entityStats;
        private EntityVFX entityVFX;
        private Slider healthBar;

        protected virtual void Awake()
        {
            entityVFX = GetComponent<EntityVFX>();
            healthBar = GetComponentInChildren<Slider>();
            entityStats = GetComponent<EntityStats.EntityStats>();
        }

        protected virtual void Start()
        {
            currentHp = entityStats.GetMaxHealth();
            UpdateHealthBar();
        }

        public virtual bool TakeDamage(float damage, Transform damageSource)
        {
            if (isDead) return false;
            if (IsAttackEvaded()) return false;
            if (!damageSource) return false; 
            
            entityVFX?.PlayKnockBackVFX(GetDirection(damageSource));
            entityVFX?.PlayOnDamageFlashVFX();
            ReduceHealth(damage);
            
            return true;
        }

        private bool IsAttackEvaded() => Random.Range(0, 100) < entityStats.GetEvasion();

        private void UpdateHealthBar()
        {
            if (healthBar == null) return;
            healthBar.value = entityStats.GetMaxHealth() > 0f ? currentHp / entityStats.GetMaxHealth() : 0f;
        }

        private void ReduceHealth(float damage)
        {
            currentHp = Mathf.Max(0f, currentHp - Mathf.Max(0f, damage));
            UpdateHealthBar();
            if (currentHp <= 0) DoDeathSequence();
        }
        
        private void DoDeathSequence()
        {  
            isDead = true;
            GetComponent<EntityController>().EntityDeath();
        }
        
        private int GetDirection(Transform source)
        {
            return transform.position.x > source.position.x ? 1 : -1;
        }
    }
}