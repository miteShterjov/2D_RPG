using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectSwordRegular : SkillObjectBase
    {
        protected SkillSwordThrow swordManager;
        protected Rigidbody2D rb;
        
        public void SetupSword(SkillSwordThrow swordSkillManager, Vector2 direction)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction;
            this.swordManager = swordSkillManager;
            entityStats = swordSkillManager.player.generalStats;
            damageScaleData = swordSkillManager.damageScaleData;
        }
    }
}