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
        private ElementType currentEffect = ElementType.None;
        
        private void Awake()
        {
            entityCombat = GetComponentInParent<EntityCombatController>();
            entityVFX = GetComponentInParent<EntityVFX>();
            generalStats = GetComponent<GeneralStats>();
        }

        public bool CanEffectBeApplied(ElementType effectType) => currentEffect == ElementType.None;

        public void ApplyChilledEffect(float duration, float moveSpeedSlowMultiplier)
        {
            float iceRes = generalStats.GetElementalResistance(ElementType.Ice);
            float reduceDuration = duration * (1 - iceRes);
            
            StartCoroutine(ChilledEffectCo(reduceDuration, moveSpeedSlowMultiplier));
        }

        private IEnumerator ChilledEffectCo(float duration, float moveSpeedSlowMultiplier)
        {
            entityCombat.SlowDownEntityBy(duration, moveSpeedSlowMultiplier);
            currentEffect = ElementType.Ice;
            entityVFX.PlayOnStatusVFX(duration, ElementType.Ice);
            yield return new WaitForSeconds(duration);
            currentEffect = ElementType.None;
        }
    }
}