using UnityEngine;
using UnityEngine.Serialization;

namespace EntityStats
{
    public class EntityStats : MonoBehaviour
    {
        public BaseStatsGroup baseStats;
        public MajorStatsGroup majorStats;
        public OffenseStatsGroup offenseStats;
        public DefenseStatsGroup defenseStats;

        public float GetMaxHealth()
        {
            float baseHealth = baseStats.maxHealth.GetValue;
            // every point of vitality adds 5 health points
            float bonusHealth = majorStats.vitality.GetValue * 5;
            
            float finalHealth = baseHealth + bonusHealth;
            
            return finalHealth;
        }

        public float GetEvasion()
        {
            float baseEvasion = defenseStats.evasion.GetValue;
            // each point of agility adds 0.5% evasion
            float bonusEvasion = majorStats.agility.GetValue * 0.5f;
            float finalEvasion = baseEvasion + bonusEvasion;
            const float evasionCap = 85f;
            float totalEvasion = Mathf.Clamp(finalEvasion, 0f, evasionCap);
           
            return finalEvasion;
        }
    }
}