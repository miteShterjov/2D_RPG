using System;
using EntityStats;

namespace Data.ElementalEffectData
{
    [Serializable]
    public class AttackData
    {
        public float physicalDamage;
        public float elementalDamage;
        public bool isCritical;
        public ElementType elementalType;
        
        public ElementalEffectData elementalEffectData;

        public AttackData(GeneralStats generalStats, DamageScaleData scaleData)
        {
            physicalDamage = generalStats.GetPhysicalDamage(out isCritical, scaleData.physicalDamage);
            elementalDamage = generalStats.GetElementalDamage(out elementalType, scaleData.elementalDamage);

            elementalEffectData = new ElementalEffectData(generalStats, scaleData);
        }
    }
}