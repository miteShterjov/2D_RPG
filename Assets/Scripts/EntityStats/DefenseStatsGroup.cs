using System;
using UnityEngine;

namespace EntityStats
{
    [Serializable]
    public class DefenseStatsGroup
    {
        public Stat armor;
        public Stat evasion;
        
        public Stat fireResistance;
        public Stat iceResistance;
        public Stat lightningResistance;
    }
}