using System;
using UnityEngine;

namespace EntityStats
{
    [Serializable]
    public class BaseStatsGroup
    {
        public Stat maxHealth;
        public Stat healthRegen;
        
        public Stat maxMana;
        public Stat manaRegen;
        
        public Stat maxStamina;
        public Stat staminaRegen;
    }
}