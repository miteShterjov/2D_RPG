namespace SkillSystem
{
    public class SkillSprint : SkillBase
    {
        public void OnStartEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.Sprint_CloneOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.Sprint_CloneOnStartAndArrival))
            {
                CreateClone();
            }
            
            if (UpgradeIsUnlocked(SkillUpgradeType.Sprint_ShardOnStart) ||
                UpgradeIsUnlocked(SkillUpgradeType.Sprint_ShardOnStartAndArrival))
            {
                CreateShard();
            }
        }

        public void OnEndEffect()
        {
            if (UpgradeIsUnlocked(SkillUpgradeType.Sprint_CloneOnStartAndArrival)) CreateClone();
            if (UpgradeIsUnlocked(SkillUpgradeType.Sprint_ShardOnStartAndArrival)) CreateShard();
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