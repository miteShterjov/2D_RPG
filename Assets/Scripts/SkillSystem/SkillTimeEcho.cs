using UnityEngine;

namespace SkillSystem
{
    public class SkillTimeEcho : SkillBase
    {
        [Header("Basic config")]
        [SerializeField] private GameObject timeEchoPrefab;
        [SerializeField] private float timeEchoDuration;
        [Header("Attack Upgrades")]
        [SerializeField] private int maxAttacks = 3;
        [SerializeField] private float duplicateChance = .3f;

        public float GetDuplicateChance()
        {
            if (upgradeType != SkillUpgradeType.TimeEchoChanceToMultiply) return 0;
            return duplicateChance;
        }

        public int GetMaxAttacks()
        {
            return upgradeType switch
            {
                SkillUpgradeType.TimeEchoSingleAttack or SkillUpgradeType.TimeEchoChanceToMultiply => 1,
                SkillUpgradeType.TimeEchoMultiAttack => maxAttacks,
                _ => 0
            };
        }

        public override void TryUseSkill()
        {
            if (!CanUseSkill()) return;
            CreateTimeEcho();
        }

        public float GetEchoDuration() => timeEchoDuration;

        public void CreateTimeEcho(Vector3? targetPosition = null)
        {
            Vector3 position = targetPosition ?? transform.position;
            GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
            timeEcho.GetComponent<SkillObjectTimeEcho>().SetupEcho(this);
        }
    }
}