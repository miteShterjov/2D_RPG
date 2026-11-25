using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectSpinSword : SkillObjectSword
    {
        private int maxDistance;
        private float attackPerSecond;
        private float attackTimer;
        private static readonly int SpinAnimEvent = Animator.StringToHash("Spin");
        
        protected override void Update()
        {
            HandleAttack();
            HandleStopping();
            HandleComeback();
        }

        public override void SetupSword(SkillSwordThrow swordSkillManager, Vector2 direction)
        {
            base.SetupSword(swordSkillManager, direction);
            
            maxDistance = swordSkillManager.maxDistance;
            attackPerSecond = swordSkillManager.attackPerSecond;
            
            if (animator != null) animator.SetTrigger(SpinAnimEvent); 
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            rb.simulated = false;
        }

        private void HandleAttack()
        {
            const float radius = 1.5f;

            if (!(attackTimer < 0)) return;
            DamageEnemiesInRange(transform, radius);
            attackTimer = 1f / attackPerSecond;
        }

        private void HandleStopping()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > maxDistance && rb.simulated == true)
            {
                rb.simulated = false;
            }
        }
    }
}