using System;
using SkillSystem;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerSkillsManager : MonoBehaviour
    {
        public SkillSprint Sprint {get; private set;}
        public SkillShard Shard {get; private set;}
        public SkillSwordThrow SwordThrow {get; private set;}
        public SkillTimeEcho TimeEcho {get; private set;}
        
        private void Awake()
        {
            Sprint = GetComponentInChildren<SkillSprint>();
            Shard = GetComponentInChildren<SkillShard>();
            SwordThrow = GetComponentInChildren<SkillSwordThrow>();
            TimeEcho = GetComponentInChildren<SkillTimeEcho>();
        }

        public SkillBase GetSkillByType(SkillType skillType)
        {
            return skillType switch
            {
                SkillType.TimeShard => Shard,
                SkillType.Sprint => Sprint,
                SkillType.SwordThrow => SwordThrow,
                SkillType.TimeEcho => TimeEcho,
                _ => throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null)
            };
        }
    }
} 