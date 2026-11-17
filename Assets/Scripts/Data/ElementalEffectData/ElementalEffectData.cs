using System;
using EntityStats;

namespace Data.ElementalEffectData
{
    [Serializable]
    public class ElementalEffectData
    {
        public float chillDuration;
        public float chillSlowEffect;

        public float burnDuration;
        public float burnDamage;

        public float shockDuration;
        public float shockDamage;
        public float shockCharge;

        public ElementalEffectData(
            GeneralStats entityStats,
            DamageScaleData damageScaleData)
        {
            chillDuration = damageScaleData.chillDuration;
            chillSlowEffect = damageScaleData.chillSlowMultiplier;
            
            burnDuration = damageScaleData.burnDuration;
            burnDamage = damageScaleData.burnDamageScale;       
            
            shockDuration = damageScaleData.shockDuration;
            shockDamage = entityStats.offenseStats.lightningDmg.GetValue() * damageScaleData.shockDamageScale;
            shockCharge = damageScaleData.shockChargeScale;
        }
    }
}