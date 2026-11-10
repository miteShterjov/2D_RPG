using System;
using SkillSystem;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerSkillsManager : MonoBehaviour
    {
        public SkillSprint Sprint {get; private set;}
        public SkillShard Shard {get; private set;}
        
        private void Awake()
        {
            Sprint = GetComponentInChildren<SkillSprint>();
            Shard = GetComponentInChildren<SkillShard>();
        }

        public SkillBase GetSkillByType(SkillType skillType)
        {
            return skillType switch
            {
                SkillType.TimeShard => Shard,
                SkillType.Sprint => Sprint,
                _ => throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null)
            };
        }
    }
}