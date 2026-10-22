using UnityEngine;

namespace SkillSystem
{
    public enum SkillUpgradeType
    {
        // ------ Sprint Tree ------
        Sprint, // Sprint to avoid enemies and damage
        Sprint_CloneOnStart, // Create a clone when Sprint starts
        Sprint_CloneOnStartAndArrival, // Create a clone when Sprint starts and ends
        Sprint_ShardOnStart, // Create a shard when Sprint starts
        Sprint_ShardOnStartAndArrival // Create a shard when Sprint starts and ends
    }
}