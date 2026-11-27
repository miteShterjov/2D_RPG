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
            if (generalStats == null) return;
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
            if (damageSource == null) return false;

            GeneralStats attackerStats = damageSource.GetComponentInParent<GeneralStats>();
            float armorPenetration = attackerStats != null ? attackerStats.GetArmorPenetration() : 0f;
            float armorMitigation = generalStats != null ? generalStats.GetArmorMitigation(armorPenetration) : 0f;
            float eleResistance = generalStats != null ? generalStats.GetElementalResistance(elementType) : 0f;
           
            float eleDmgTaken = elementalDamage * (1 - eleResistance);
            
            float physicalDmg = damage * (1 - armorMitigation);
            
            entityVFX?.PlayKnockBackVFX(GetDirection(damageSource));
            
            ReduceHealth(physicalDmg + eleDmgTaken);
            return true;
        }

        public float GetHealthPercentage()
        {
            if (generalStats == null) return 0;
            return currentHealth / generalStats.GetMaxHealth();
        }
        
        public void SetHealthToPercentage(float percentage) 
        {
            if (generalStats == null) return;
            currentHealth = generalStats.GetMaxHealth() * Mathf.Clamp01(percentage);
            UpdateHealthBar();
        }
        
        public void ReduceHealth(float damage)
        {
            entityVFX?.PlayOnDamageFlashVFX();
            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Max(0f, damage));
            UpdateHealthBar();
            if (currentHealth <= 0) DoDeathSequence();
        }
        
        private void IncreaseHealth(float regenAmount)
        {
            if (isDead) return;
            if (generalStats == null) return;
            float newHealth = currentHealth + regenAmount;
            float maxHealth = generalStats.GetMaxHealth();
            
            currentHealth = Mathf.Min(newHealth, maxHealth);
            UpdateHealthBar();
        }

        private void RegenHealth()
        {
            if (!canRegen) return;
            if (generalStats == null) return;
            float regenAmount = generalStats.baseStats.healthRegen.GetValue();
            IncreaseHealth(regenAmount);
        }

        private bool IsAttackEvaded() 
        {
            if (generalStats == null) return  false;
            else return Random.Range(0, 100) < generalStats.GetEvasion();
        }

        private void UpdateHealthBar()
        {
            if (healthBar == null) return;
            if (generalStats == null) return;
            healthBar.value = generalStats.GetMaxHealth() > 0f ? currentHealth / generalStats.GetMaxHealth() : 0f;
        }

        protected virtual void DoDeathSequence()
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