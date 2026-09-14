using EntityControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyCollisionController : EntityCollisionController
    {
        [Header("Collision Config")]
        [SerializeField] protected Transform primaryGroundCheck;
        [SerializeField] protected float aggroRange = 10f;
        [SerializeField] protected LayerMask playerMask;
        
        public Transform DetectedPlayer { get; private set; }
        public bool isPlayerDetected;

        private EnemyController enemyController;
        private const string PlayerTag = "Player";

        protected override void Awake()
        {
            base.Awake();
            enemyController = GetComponent<EnemyController>();
        }

        protected override void Update()
        {
            HandlePlayerDetection();
            HandleGroundCollision();
            HandleWallCollision();
            HandlePlayerCollision();
        }

        private void HandlePlayerDetection()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Entity.entityMove.FacingDir,
                aggroRange, playerMask | groundMask);
            bool hitPlayer = hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer(PlayerTag);
            if (hitPlayer)
            {
                EntityHealthController targetHealth = hit.collider.GetComponent<EntityHealthController>();
                hitPlayer = targetHealth == null || !targetHealth.isDead;
            }

            isPlayerDetected = hitPlayer;
            DetectedPlayer = hitPlayer ? hit.collider.transform : null;
        }

        private void HandleGroundCollision()
        {
            isGrounded = Physics2D.Raycast(primaryGroundCheck.position, Vector2.down, groundCheckDistance, groundMask);
        }

        private void HandleWallCollision()
        {
            isWallDetected =
                Physics2D.Raycast(primaryWallCheck.transform.position, Vector2.right * Entity.entityMove.FacingDir,
                    wallCheckDistance, groundMask) && Physics2D.Raycast(secondaryWallCheck.transform.position,
                    Vector2.right * Entity.entityMove.FacingDir, wallCheckDistance, groundMask);
        }

        private void HandlePlayerCollision()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Entity.entityMove.FacingDir,
                aggroRange, playerMask | groundMask);
            bool hitPlayer = hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer(PlayerTag);
            if (hitPlayer)
            {
                EntityHealthController targetHealth = hit.collider.GetComponent<EntityHealthController>();
                hitPlayer = targetHealth == null || !targetHealth.isDead;
            }

            isPlayerDetected = hitPlayer;
        }

        protected override void OnDrawGizmos()
        {
            if (Entity == null) Entity = GetComponent<EntityController>();
            if (enemyController == null) enemyController = GetComponent<EnemyController>();
            if (Entity == null || Entity.entityMove == null) return;
            if (enemyController == null) return;
            if (primaryGroundCheck == null || primaryWallCheck == null || secondaryWallCheck == null) return;
            
            const float wireSphereRadius = 0.09f;
            
            DrawGroundCheckGizmos(wireSphereRadius);
            DrawWallCheckGizmos(wireSphereRadius);
            DrawAggroGizmos(wireSphereRadius);
            DrawAttackRangeGizmos(wireSphereRadius);
            DrawRetreatDistanceGizmo();
        }

        private void DrawRetreatDistanceGizmo()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyController.minRetreatDistance);
        }

        private void DrawAttackRangeGizmos(float wireSphereRadius)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + (Entity.entityMove.FacingDir * enemyController.attackDistance),
                    transform.position.y));
            Gizmos.DrawWireSphere(
                new Vector3(transform.position.x + (Entity.entityMove.FacingDir * enemyController.attackDistance),
                    transform.position.y), wireSphereRadius);
        }

        private void DrawAggroGizmos(float wireSphereRadius)
        {
            Gizmos.color = isPlayerDetected ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.right * aggroRange * Entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * aggroRange * Entity.entityMove.FacingDir,
                wireSphereRadius);
        }

        private void DrawWallCheckGizmos(float wireSphereRadius)
        {
            Gizmos.color = isWallDetected ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryWallCheck.position,
                primaryWallCheck.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(
                primaryWallCheck.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir,
                wireSphereRadius);
            Gizmos.DrawLine(secondaryWallCheck.position,
                secondaryWallCheck.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(
                secondaryWallCheck.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir,
                wireSphereRadius);
        }

        private void DrawGroundCheckGizmos(float wireSphereRadius)
        {
            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryGroundCheck.position,
                primaryGroundCheck.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(primaryGroundCheck.position + Vector3.down * groundCheckDistance, wireSphereRadius);
        }
    }
}