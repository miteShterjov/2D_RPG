using System.Collections;
using EntityStats;
using UnityEngine;

namespace EntityControl
{
    public class EntityStatusHandler : MonoBehaviour
    {
        [Header("Thunder Strike")]
        [SerializeField] private GameObject thunderStrikeEffectPrefab;
        [SerializeField] private float currentCharge;
        [SerializeField] private float maxCharges = 1f;
        
        private EntityCombatController entityCombat;
        private GeneralStats generalStats;
        private EntityVFX entityVFX;
        private EntityHealthController entityHealth;
        private ElementType currentEffect = ElementType.None;
        private Coroutine electrifyCo;
        
        private void Awake()
        {
            entityCombat = GetComponentInParent<EntityCombatController>();
            entityVFX = GetComponentInParent<EntityVFX>();
            generalStats = GetComponent<GeneralStats>();
            entityHealth = GetComponent<EntityHealthController>();
        }

        public bool CanEffectBeApplied(ElementType effectType)
        {
            // when/if fully charged discharge thunder strike
            if (effectType == ElementType.Lightning && currentEffect == ElementType.Lightning) return true;
            
            return currentEffect == ElementType.None;
        }

        public void ApplyChillEffect(float duration, float moveSpeedSlowMultiplier)
        {
            float iceRes = generalStats.GetElementalResistance(ElementType.Ice);
            float reduceDuration = duration * (1f - iceRes);
            
            StartCoroutine(ChillEffectCo(reduceDuration, moveSpeedSlowMultiplier));
        }
        
        public void ApplyBurningEffect(float duration, float totalDamage)
        {
            float fireRes = generalStats.GetElementalResistance(ElementType.Fire);
            float finalDmg = totalDamage * (1f - fireRes);
            
            StartCoroutine(BurningEffectCo(duration, finalDmg));
        }

        public void ApplyLightningEffect(float duration, float damage, float charge)
        {
            float lightningRes = generalStats.GetElementalResistance(ElementType.Lightning);
            float finalCharge = charge * (1f - lightningRes);
            
            currentCharge += charge + finalCharge;
            
            if (currentCharge >= maxCharges)
            {
                DoThunderStrikeEffect(damage);
                StopElectrifyEffect();
                return;
            }
            
            if (electrifyCo != null) StopCoroutine(electrifyCo);
            electrifyCo = StartCoroutine(ElectrifyEffectCo(duration));
        }
        
        public void StopAllStatusEffects()
        {
            StopCoroutine(electrifyCo);
            entityVFX.StopAllStatusEffects();
            currentEffect = ElementType.None;
            currentCharge = 0;
        }
        
        private IEnumerator ChillEffectCo(float duration, float moveSpeedSlowMultiplier)
        {
            entityCombat.SlowDownEntityBy(duration, moveSpeedSlowMultiplier);
            currentEffect = ElementType.Ice;
            entityVFX.PlayOnStatusVFX(duration, ElementType.Ice);
            yield return new WaitForSeconds(duration);
            currentEffect = ElementType.None;
        }

        private IEnumerator BurningEffectCo(float duration, float totalDamage)
        {
            currentEffect = ElementType.Fire;
            entityVFX.PlayOnStatusVFX(duration, ElementType.Fire);

            const int ticksPerSecond = 2;
            int ticksCount = Mathf.RoundToInt(ticksPerSecond * duration);

            float damagerPerTick = totalDamage / ticksCount;
            // ReSharper disable once PossibleLossOfFraction
            const float ticksInterval = 1f / ticksPerSecond;

            for (int i = 0; i < ticksCount; i++)
            {
                entityHealth.ReduceHealth(damagerPerTick);
                yield return new WaitForSeconds(ticksInterval);
            }
            
            currentEffect = ElementType.None;
        }

        private IEnumerator ElectrifyEffectCo(float duration)
        {
            currentEffect = ElementType.Lightning;
            entityVFX.PlayOnStatusVFX(duration, ElementType.Lightning);
            
            yield return new WaitForSeconds(duration);
            StopElectrifyEffect();
        }
        
        private void StopElectrifyEffect()
        {
            currentEffect = ElementType.None;
            currentCharge = 0;
            entityVFX.StopAllStatusEffects();
        }
        
        private void DoThunderStrikeEffect(float damage)
        {
            Instantiate(thunderStrikeEffectPrefab, transform.position, Quaternion.identity);
            entityHealth.ReduceHealth(damage);
        }
    }
}