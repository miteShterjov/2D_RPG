using UnityEngine;

namespace EntityStats
{
    [CreateAssetMenu(fileName = "Default Stat Setup", menuName = "RPG Setup/Stat Setup", order = 0)]
    public class StatSetupSo : ScriptableObject
    {
        [Header("Resources")]
        public float maxHealth;
        public float healthRegen;

        [Header("Offense - Physical Damage")]
        public float attackSpeed;
        public float damage;
        public float critChance;
        public float critPower;
        public float armorReduction;

        [Header("Offense - Elemental Damage")]
        public float fireDamage;
        public float iceDamage;
        public float lightningDamage;

        [Header("Defense - Physical Damage")]
        public float armor;
        public float evasion;

        [Header("Defense - Elemental Damage")]
        public float fireResistance;
        public float iceResistance;
        public float lightningResistance;
        
        [Header("Major Stats")]
        public float strength;
        public float agility;
        public float intelligence;
        public float vitality;
    }
}