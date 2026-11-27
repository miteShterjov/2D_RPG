using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectTimeEcho : SkillObjectBase
    {
        [Header("Echo Settings")]
        [SerializeField] private float groundCheckDistance = 1.5f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private GameObject onDeathVisual;

        public int MaxAttacks { get; private set; }

        private SkillTimeEcho echoManager;
        private int attacksPerformed;
        private bool isDying;
        private const int MaxAnimationAttacks = 3;
        private TrailRenderer wispTrail;
        
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int Attack1State = Animator.StringToHash("Base Layer.Attack1");
        private static readonly int Attack2State = Animator.StringToHash("Base Layer.Attack2");
        private static readonly int Attack3State = Animator.StringToHash("Base Layer.Attack3");

        protected override void Awake()
        {
            base.Awake();
            wispTrail = GetComponentInChildren<TrailRenderer>();
        }

        private void Update()
        {
            animator.SetFloat(YVelocity, rb.linearVelocity.y);
            StopHorizontalMovement();
        }

        public void SetupEcho(SkillTimeEcho timeEcho)
        {
            echoManager = timeEcho;
            entityStats = timeEcho.player.generalStats;
            damageScaleData = timeEcho.damageScaleData;
            MaxAttacks = Mathf.Clamp(echoManager.GetMaxAttacks(), 0, MaxAnimationAttacks);
            
            FlipSpriteTowardTarget();
            
            if (MaxAttacks > 0)
                animator.Play(Attack1State, 0, 0f);
            else
                Invoke(nameof(HandleEchoDeath), timeEcho.GetEchoDuration());
        }

        public void HandleEchoDeath()
        {
            if (isDying) return;
            isDying = true;
            CancelInvoke(nameof(HandleEchoDeath));
            Instantiate(onDeathVisual, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void PreformAttack()
        {
            if (attacksPerformed >= MaxAttacks) return;
            attacksPerformed++;
            DamageEnemiesInRange(targetCheck, 1);
            
            if (!targetGotHit) return;
            bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
            float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

            if (canDuplicate) echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0, 0));
        }

        public void TryTerminate(int currentAttackIndex)
        {
            if (isDying) return;
            if (currentAttackIndex >= MaxAttacks)
            {
                HandleEchoDeath();
                return;
            }

            int nextAttackState = currentAttackIndex switch
            {
                1 => Attack2State,
                2 => Attack3State,
                _ => 0
            };

            if (nextAttackState != 0)
                animator.Play(nextAttackState, 0, 0f);
            else
                HandleEchoDeath();
        }

        private void StopHorizontalMovement()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                Vector2.down,
                groundCheckDistance,
                whatIsGround);

            if (hit.collider != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        private void FlipSpriteTowardTarget()
        {
            Transform target = FindNearestTarget();
            if (target.position.x < transform.position.x) transform.Rotate(0, 180, 0);
        }

    }
}