using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectSwordPierce : SkillObjectSword
    {
        private int amountToPierce;

        public override void SetupSword(SkillSwordThrow swordSkillManager, Vector2 direction)
        {
            base.SetupSword(swordSkillManager, direction);
            amountToPierce = swordSkillManager.pierceAmountOfTargets;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
            const float radius = .3f;

            if (amountToPierce <= 0 || groundHit)
            {
                DamageEnemiesInRange(transform, radius);
                StopSword(collision);
                return;
            }
            
            amountToPierce--;
            DamageEnemiesInRange(transform, radius);
        }
        
        
    }
}