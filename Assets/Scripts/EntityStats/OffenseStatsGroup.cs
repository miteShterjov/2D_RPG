using System;
using UnityEngine;

namespace EntityStats
{
    [Serializable]
    public class OffenseStatsGroup
    {
        public Stat damage;
        public Stat critPower;
        public Stat critChance;

        public Stat armorPenetration;

        public Stat fireDmg;
        public Stat iceDmg;
        public Stat lightningDmg;
    }
}