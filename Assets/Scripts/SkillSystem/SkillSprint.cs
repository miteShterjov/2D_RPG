namespace SkillSystem
{
    public class SkillSprint : SkillBase
    {
        public void OnStartEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStartAndArrival))
            {
                CreateClone();
            }
            
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStartAndArrival))
            {
                CreateShard();
            }
        }

        public void OnEndEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintCloneOnStartAndArrival)) CreateClone();
            if (UpgradeIsUnlocked(SkillUpgradeType.SprintShardOnStartAndArrival)) CreateShard();
        }
        
        private static void CreateShard()
        {
            print("Creating shard");
            //todo: create shard logic
        }

        private static void CreateClone()
        {
            print("Creating time echo");
            //todo: create time echo logic
        }
    }
}