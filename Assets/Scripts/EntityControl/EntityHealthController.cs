using Interface;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace EntityControl
{
    public class EntityHealthController : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] public bool isDead;

        private EntityVFX entityVFX;
        private Slider healthBar;
        private float currentHp;

        protected virtual void Awake()
        {
            entityVFX = GetComponent<EntityVFX>();
            healthBar = GetComponentInChildren<Slider>();
        }

        protected virtual void Start()
        {
            currentHp = maxHealth;
            UpdateHealthBar();
        }

        public virtual void TakeDamage(float damage, Transform damageSource)
        {
            if (isDead) return;
            
            entityVFX?.PlayKnockBackVFX(GetDirection(damageSource));
            entityVFX?.PlayOnDamageFlashVFX();
            ReduceHealth(damage);
        }

        private void UpdateHealthBar() => healthBar.value = currentHp / maxHealth;

        private void ReduceHealth(float damage)
        {
            currentHp -= damage;
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