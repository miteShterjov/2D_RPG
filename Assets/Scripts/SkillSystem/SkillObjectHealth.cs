using EntityControl;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectHealth : EntityHealthController
    {
        protected override void DoDeathSequence()
        {
            SkillObjectTimeEcho timeEcho = GetComponent<SkillObjectTimeEcho>();
            timeEcho.HandleEchoDeath();
        }
    }
}