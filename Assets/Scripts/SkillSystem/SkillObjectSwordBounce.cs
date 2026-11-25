using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectSwordBounce : SkillObjectSword
    {
        private float bounceSpeed;
        private int bounceCount;
        private Transform nextTarget;
        private Transform currentTarget;
        private List<Collider2D> enemyTargets;
        private List<Transform> taggedTargets = new List<Transform>();
        private static readonly int SpinAnimEvent = Animator.StringToHash("Spin");

        protected override bool RequiresAnimator => true;

        protected override void Update()
        {
            HandleComeback();
            HandleBounce();
        }

        public override void SetupSword(SkillSwordThrow swordSkillManager, Vector2 direction)
        {
            animator.SetTrigger(SpinAnimEvent);
            base.SetupSword(swordSkillManager, direction);
            bounceSpeed = swordSkillManager.bounceSpeed;
            bounceCount = swordSkillManager.bounceCount;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (enemyTargets != null) return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                StopSword(other);
                GetSwordBackToPlayer();
                return;
            }

            if ((enemyMask.value & (1 << other.gameObject.layer)) == 0) return;

            const float radius = 10f;
            enemyTargets = new List<Collider2D>(GetEnemiesAround(transform, radius));
            currentTarget = other.transform;
            taggedTargets.Add(currentTarget);
            rb.simulated = false;
            
            DamageEnemiesInRange(transform, 1);
            
            if (!BounceToNextTarget()) GetSwordBackToPlayer();
        }

        private List<Transform> GetAliveTargets()
        {
            return (from enemy in enemyTargets where enemy != null select enemy.transform).Distinct().ToList();
        }
        
        private List<Transform> GetValidTargets()
        {
            List<Transform> aliveTargets = GetAliveTargets();
            List<Transform> validTargets = aliveTargets
                .Where(target => target != currentTarget && !taggedTargets.Contains(target))
                .ToList();
            if (validTargets.Count > 0) return validTargets;

            taggedTargets.Clear();
            if (currentTarget != null) taggedTargets.Add(currentTarget);
            return aliveTargets.Where(target => target != currentTarget).ToList();
        }
        
        private Transform GetNextTarget()
        {
            List<Transform> validTargets = GetValidTargets();
            if (validTargets.Count == 0) return null;

            int randomIndex = Random.Range(0, validTargets.Count);
            
            Transform target = validTargets[randomIndex];
            taggedTargets.Add(target);

            return target;
        }

        private bool BounceToNextTarget()
        {
            if (bounceCount <= 0) return false;

            nextTarget = GetNextTarget();
            if (nextTarget == null) return false;

            bounceCount--;
            return true;
        }

        private void HandleBounce()
        {
            if (nextTarget == null) return;
            transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);

            if (!(Vector2.Distance(transform.position, nextTarget.position) < 0.75f)) return;
            currentTarget = nextTarget;
            DamageEnemiesInRange(transform, 1);

            if (BounceToNextTarget()) return;
            nextTarget = null;
            GetSwordBackToPlayer();
        }
    }
}