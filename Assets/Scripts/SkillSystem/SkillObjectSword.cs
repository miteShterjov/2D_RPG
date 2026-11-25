using UnityEngine;

namespace SkillSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SkillObjectSword : SkillObjectBase
    {
        protected override bool RequiresAnimator => false;

        protected SkillSwordThrow swordManager;
        protected Transform playerTransform;
        protected Rigidbody2D rb;
        
        private bool shouldComeBack;
        private readonly float comeBackSpeed = 20f;
        private readonly float maxDistanceAllowed = 30f;
        

        protected virtual void Update()
        {
            transform.right = rb.linearVelocity.normalized;
            HandleComeback();
        }

        public virtual void SetupSword(SkillSwordThrow swordSkillManager, Vector2 direction)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction;
            this.swordManager = swordSkillManager;
            playerTransform = swordSkillManager.transform.root;
            entityStats = swordSkillManager.player.generalStats;
            damageScaleData = swordSkillManager.damageScaleData;
        }
        
        public void GetSwordBackToPlayer() => shouldComeBack = true;

        protected void HandleComeback()
        {
            if (!shouldComeBack) return;
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comeBackSpeed * Time.deltaTime);
            if (distance > maxDistanceAllowed) GetSwordBackToPlayer();
            if (distance < 0.5f) Destroy(gameObject);
        }

        protected void StopSword(Collider2D collision)
        {
            rb.simulated = false;
            transform.parent = collision.transform;

            if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;

            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError($"SpriteRenderer not found on {GetType().Name} after hitting Ground", this);
                return;
            }

            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = -1;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            const float radius = 1f;
            StopSword(other);
            DamageEnemiesInRange(transform, radius);
        }
        
        
    }
}