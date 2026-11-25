namespace SkillSystem
{
    public enum SkillUpgradeType
    {
        None, 
        
        // ------ Sprint Tree ------
        Sprint, // Sprint to avoid enemies and damage
        SprintCloneOnStart, // Create a clone when Sprint starts
        SprintCloneOnStartAndArrival, // Create a clone when Sprint starts and ends
        SprintShardOnStart, // Create a shard when Sprint starts
        SprintShardOnStartAndArrival, // Create a shard when Sprint starts and ends
        
        // ------ Shard Tree ------
        Shard, // The shard explodes when touched by an enemy or time goes up
        ShardMoveToEnemy, // Shard will move towards nearest enemy
        ShardMultiCast, // Shard ability can have up to N charges. You can cast them all in a row
        ShardTeleport, // You can swap places with the last shard you created
        ShardTeleportAndHpRewind, // When you swap places with shard, your HP % is same as it was when you created shard.
        
        // ------ Sword Throw Tree ------
        SwordThrow, // Player throws the sword to damage enemies from a distance
        SwordThrowSpin, // Player throws the sword to damage enemies from a distance and spins around like a chainsaw
        SwordThrowPierce, // Player throws the sword to damage enemies from a distance and pierces through N enemies
        SwordThrowBounce, // Player throws the sword to damage enemies from a distance and bounces off N enemies
        
    }
}