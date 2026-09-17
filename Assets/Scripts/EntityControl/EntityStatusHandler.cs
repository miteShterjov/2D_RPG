using System.Collections;
using EntityStats;
using UnityEngine;

namespace EntityControl
{
    public class EntityStatusHandler : MonoBehaviour
    {
        private EntityCombatController entityCombat;
        private GeneralStats generalStats;
        private EntityVFX entityVFX;
        private EntityHealthController entityHealth;
        private ElementType currentEffect = ElementType.None;
        
        private void Awake()
        {
            entityCombat = GetComponentInParent<EntityCombatController>();
            entityVFX = GetComponentInParent<EntityVFX>();
            generalStats = GetComponent<GeneralStats>();
            entityHealth = GetComponent<EntityHealthController>();
        }

        public bool CanEffectBeApplied(ElementType effectType) => currentEffect == ElementType.None;

        public void ApplyChilledEffect(float duration, float moveSpeedSlowMultiplier)
        {
            float iceRes = generalStats.GetElementalResistance(ElementType.Ice);
            float reduceDuration = duration * (1f - iceRes);
            
            StartCoroutine(ChilledEffectCo(reduceDuration, moveSpeedSlowMultiplier));
        }
        
        public void ApplyBurningEffect(float duration, float totalDamage)
        {
            float fireRes = generalStats.GetElementalResistance(ElementType.Fire);
            float finalDmg = totalDamage * (1f - fireRes);
            
            StartCoroutine(BurningEffectCo(duration, finalDmg));
        }

        private IEnumerator ChilledEffectCo(float duration, float moveSpeedSlowMultiplier)
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
    }
}