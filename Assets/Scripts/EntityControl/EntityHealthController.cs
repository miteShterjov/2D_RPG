using EntityStats;
using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace EntityControl
{
    public class EntityHealthController : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private float currentHealth;
        [SerializeField] public bool isDead;

        [Header("Health Reget")] [SerializeField]
        private float regenInterval = 1;
        [SerializeField] private bool canRegen = true;
        
        private GeneralStats generalStats;
        private EntityVFX entityVFX;
        private Slider healthBar;

        protected virtual void Awake()
        {
            entityVFX = GetComponent<EntityVFX>();
            healthBar = GetComponentInChildren<Slider>();
            generalStats = GetComponent<GeneralStats>();
        }

        protected virtual void Start()
        {
            currentHealth = generalStats.GetMaxHealth();
            UpdateHealthBar();
            InvokeRepeating(nameof(RegenHealth), 0, regenInterval);
        }

        public virtual bool TakeDamage(
            float damage, 
            float elementalDamage, 
            ElementType elementType, 
            Transform damageSource)
        {
            if (isDead) return false;
            if (IsAttackEvaded()) return false;
            if (!damageSource) return false;

            float armorPenetration = damageSource.GetComponent<GeneralStats>().GetArmorPenetration();
            float armorMitigation = generalStats.GetArmorMitigation(armorPenetration);
           
            float eleResistance = generalStats.GetElementalResistance(elementType);
            float eleDmgTaken = elementalDamage * (1 - eleResistance);
            
            float physicalDmg = damage * (1 - armorMitigation);
            
            entityVFX?.PlayKnockBackVFX(GetDirection(damageSource));
            
            ReduceHealth(physicalDmg + eleDmgTaken);
            return true;
        }

        public void IncreaseHealth(float regenAmount)
        {
            if (isDead) return;

            float newHealth = currentHealth + regenAmount;
            float maxHealth = generalStats.GetMaxHealth();
            
            currentHealth = Mathf.Min(newHealth, maxHealth);
            UpdateHealthBar();
        }
        
        public void ReduceHealth(float damage)
        {
            entityVFX?.PlayOnDamageFlashVFX();
            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Max(0f, damage));
            UpdateHealthBar();
            if (currentHealth <= 0) DoDeathSequence();
        }

        private void RegenHealth()
        {
            if (!canRegen) return;
            
            float regenAmount = generalStats.baseStats.healthRegen.GetValue();
            IncreaseHealth(regenAmount);
        }

        private bool IsAttackEvaded() => Random.Range(0, 100) < generalStats.GetEvasion();

        private void UpdateHealthBar()
        {
            if (healthBar == null) return;
            healthBar.value = generalStats.GetMaxHealth() > 0f ? currentHealth / generalStats.GetMaxHealth() : 0f;
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