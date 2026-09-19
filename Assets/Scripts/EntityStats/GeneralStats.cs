using UnityEngine;
using Random = UnityEngine.Random;

namespace EntityStats
{
    public class GeneralStats : MonoBehaviour
    {
        public StatSetupSo defaultStatSetup;
        public ElementType elementType;
        public BaseStatsGroup baseStats;
        public OffenseStatsGroup offenseStats;
        public DefenseStatsGroup defenseStats;
        public MajorStatsGroup majorStats;

        public float GetMaxHealth()
        {
            float baseHealth = baseStats.maxHealth.GetValue();
            // every point of vitality adds 5 health points
            float bonusHealth = majorStats.vitality.GetValue() * 5;
            
            float finalHealth = baseHealth + bonusHealth;
            
            return finalHealth;
        }

        public float GetArmorMitigation(float armorReduction = 0f)
        {
            float baseArmor = defenseStats.armor.GetValue();
            // bonus 1 armor point for every vitality point
            float bonusBaseArmor = majorStats.vitality.GetValue();
            float finalBaseArmor = baseArmor + bonusBaseArmor;
            
            float armorReductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
            float effectiveArmor = finalBaseArmor * armorReductionMultiplier;
            
            float mitigation = effectiveArmor / (effectiveArmor + 100);
            const float mitigationCap = 0.8f;
            
            float finalMitigation = Mathf.Clamp(mitigation, 0f, mitigationCap);

            return finalMitigation;
        }

        public float GetEvasion()
        {
            float baseEvasion = defenseStats.evasion.GetValue();
            // each point of agility adds 0.5% evasion
            float bonusEvasion = majorStats.agility.GetValue() * 0.5f;
            float finalEvasion = baseEvasion + bonusEvasion;
            const float evasionCap = 85f;
            float totalEvasion = Mathf.Clamp(finalEvasion, 0f, evasionCap);
           
            return totalEvasion;
        }

        public float GetPhysicalDamage(out bool isCrit)
        {
            float baseDamage = offenseStats.damage.GetValue();
            // each point of strength adds 1 damage point
            float bonusDamage = majorStats.strength.GetValue();
            float totalBaseDamage = baseDamage + bonusDamage;
            
            float baseCritChance = offenseStats.critChance.GetValue();
            // each point of agility adds 0.3% crit chance
            float bonusCritChance = majorStats.agility.GetValue() * 0.3f;
            float totalCritChance = baseCritChance + bonusCritChance;

            float baseCritPower = offenseStats.critPower.GetValue();
            // every point of strength adds +0.5% to crit power
            float bonusCritPower = majorStats.strength.GetValue() * 0.05f;
            // total crit power as multiplier of base crit power
            float totalCritPower = (baseCritPower + bonusCritPower) / 100;
            
            isCrit = Random.Range(0, 100) < totalCritChance;
            float finalDamage = isCrit ? totalBaseDamage * totalCritPower : totalBaseDamage;
            
            return finalDamage;
        }

        public float GetArmorPenetration()
        {
            // armor pen as multiplier stat
            float armorPen = offenseStats.armorPenetration.GetValue() / 100;
            const float armorPenCap = 0.6f;
            
            float finalArmorPen = Mathf.Clamp(armorPen, 0f, armorPenCap);
            
            return finalArmorPen;
        }

        public float GetElementalDamage(out ElementType element)
        {
            float fireDmg = offenseStats.fireDmg.GetValue();
            float iceDmg = offenseStats.iceDmg.GetValue();
            float staticDmg = offenseStats.lightningDmg.GetValue();
            // bonus 1 point per 1 point of int
            float bonusEleDmg = majorStats.intelligence.GetValue();
            
            float highestDmg = Mathf.Max(fireDmg, iceDmg, staticDmg);
            element = Mathf.Approximately(fireDmg, highestDmg) ? ElementType.Fire : ElementType.None;
            element = Mathf.Approximately(iceDmg, highestDmg) ? ElementType.Ice : element;
            element = Mathf.Approximately(staticDmg, highestDmg) ? ElementType.Lightning : element;
            element = Mathf.Approximately(highestDmg, 0) ? ElementType.None : element;
            
            // the other values that are not max give bonus eleDmg
            // equal to 50% of their value
            float bonusFireDmg = (Mathf.Approximately(fireDmg, highestDmg)) ? 0 : fireDmg * 0.5f;
            float bonusIceDmg = (Mathf.Approximately(iceDmg, highestDmg)) ? 0 : iceDmg * 0.5f;
            float bonusStaticDmg = (Mathf.Approximately(staticDmg, highestDmg)) ? 0 : staticDmg * 0.5f;
            float bonusLesserEleDmg = bonusFireDmg + bonusIceDmg + bonusStaticDmg;
            
            if (highestDmg <= 0) return 0;
            
            float finalDmg = highestDmg + bonusEleDmg + bonusLesserEleDmg;
            return finalDmg;
        }

        public float GetElementalDamage()
        {
            float fireDmg = offenseStats.fireDmg.GetValue();
            float iceDmg = offenseStats.iceDmg.GetValue();
            float staticDmg = offenseStats.lightningDmg.GetValue();
            // bonus 1 point per 1 point of int
            float bonusEleDmg = majorStats.intelligence.GetValue();
            
            float highestDmg = Mathf.Max(fireDmg, iceDmg, staticDmg);
            // the other values that are not max give bonus eleDmg
            // equal to 50% of their value
            float bonusFireDmg = (Mathf.Approximately(fireDmg, highestDmg)) ? 0 : fireDmg * 0.5f;
            float bonusIceDmg = (Mathf.Approximately(iceDmg, highestDmg)) ? 0 : iceDmg * 0.5f;
            float bonusStaticDmg = (Mathf.Approximately(staticDmg, highestDmg)) ? 0 : staticDmg * 0.5f;
            float bonusLesserEleDmg = bonusFireDmg + bonusIceDmg + bonusStaticDmg;
            
            if (highestDmg <= 0) return 0;
            
            float finalDmg = highestDmg + bonusEleDmg + bonusLesserEleDmg;
            return finalDmg;
        }

        public float GetElementalResistance(ElementType element)
        {
            float baseResistance;
            // each point of intelligence adds 0.5% resistance
            float bonusResistance = majorStats.intelligence.GetValue() * .5f;
            
            switch (element)
            {
                case ElementType.Fire:
                    baseResistance = defenseStats.fireResistance.GetValue();
                    break;
                case ElementType.Ice:
                    baseResistance = defenseStats.iceResistance.GetValue();
                    break;
                case ElementType.Lightning:
                    baseResistance = defenseStats.lightningResistance.GetValue();
                    break;
                default:
                case ElementType.None:
                    return 0;
            }

            float eleResistance = baseResistance + bonusResistance;
            const float resistanceCap = 75f;
            float finalResistance = Mathf.Clamp(eleResistance, 0f, resistanceCap) / 100;
            return finalResistance;
        }

        public Stat GetStatByType(StatType type)
        {
            switch (type)
            {
                case StatType.MaxHealth: return baseStats.maxHealth;
                case StatType.HealthRegen: return baseStats.healthRegen;
                case StatType.MaxMana: return baseStats.maxMana;
                case StatType.ManaRegen: return baseStats.manaRegen;
                case StatType.MaxStamina: return baseStats.maxStamina;
                case StatType.StaminaRegen: return baseStats.staminaRegen;
                case StatType.Strength: return majorStats.strength;
                case StatType.Agility: return majorStats.agility;
                case StatType.Intelligence: return majorStats.intelligence;
                case StatType.Vitality: return majorStats.vitality;
                case StatType.AttackSpeed: return offenseStats.attackSpeed;
                case StatType.Damage: return offenseStats.damage;
                case StatType.CritPower: return offenseStats.critPower;
                case StatType.CritChance: return offenseStats.critChance;
                case StatType.ArmorReduction: return offenseStats.armorPenetration;
                case StatType.FireDamage: return offenseStats.fireDmg;
                case StatType.IceDamage: return offenseStats.iceDmg;
                case StatType.LightningDamage: return offenseStats.lightningDmg;
                case StatType.Armor: return defenseStats.armor;
                case StatType.Evasion: return defenseStats.evasion;
                case StatType.FireResistance: return defenseStats.fireResistance;
                case StatType.IceResistance: return defenseStats.iceResistance;
                case StatType.LightningResistance: return defenseStats.lightningResistance;
                    
                default:
                    Debug.LogWarning($"StatType {type} not implemented so far.");
                    return null;
            }
        }
        
        [ContextMenu("Apply Default Stat Setup")]
        public void ApplyDefaultStatSetup()
        {
            if (!defaultStatSetup)
            {
                Debug.LogWarning("No default stat setup found.");
                return;
            }

            baseStats.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
            baseStats.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);
            
            majorStats.strength.SetBaseValue(defaultStatSetup.strength);
            majorStats.agility.SetBaseValue(defaultStatSetup.agility);
            majorStats.intelligence.SetBaseValue(defaultStatSetup.intelligence);
            majorStats.vitality.SetBaseValue(defaultStatSetup.vitality);
            
            offenseStats.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
            offenseStats.damage.SetBaseValue(defaultStatSetup.damage);
            offenseStats.critChance.SetBaseValue(defaultStatSetup.critChance);
            offenseStats.critPower.SetBaseValue(defaultStatSetup.critPower);
            offenseStats.armorPenetration.SetBaseValue(defaultStatSetup.armorReduction);
            
            offenseStats.fireDmg.SetBaseValue(defaultStatSetup.fireDamage);
            offenseStats.iceDmg.SetBaseValue(defaultStatSetup.iceDamage);
            offenseStats.lightningDmg.SetBaseValue(defaultStatSetup.lightningDamage);
            
            defenseStats.armor.SetBaseValue(defaultStatSetup.armor);
            defenseStats.evasion.SetBaseValue(defaultStatSetup.evasion);
            
            defenseStats.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
            defenseStats.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
            defenseStats.lightningResistance.SetBaseValue(defaultStatSetup.lightningResistance);
        }
    }
}