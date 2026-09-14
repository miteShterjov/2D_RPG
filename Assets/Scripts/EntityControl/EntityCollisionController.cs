using UnityEngine;

namespace EntityControl
{
    public class EntityCollisionController : MonoBehaviour
    {
        public bool IsGrounded => isGrounded;
        public bool IsWallDetected => isWallDetected;
        
        [Header("Collision")]
        [SerializeField] protected float groundCheckDistance = 1.4f;
        [SerializeField] protected float wallCheckDistance = 1.4f;
        [SerializeField] protected LayerMask groundMask;
        [SerializeField] protected bool isGrounded;
        [SerializeField] protected bool isWallDetected;
        [SerializeField] protected Transform primaryWallCheck;
        [SerializeField] protected Transform secondaryWallCheck;

        protected EntityController Entity;

        protected virtual void Awake()
        {
            Entity = GetComponent<EntityController>();
        }

        protected virtual void Update()
        {
            HandleCollisionDetection();
        }

        private void HandleCollisionDetection()
        {
            isGrounded = Physics2D.Raycast(
                transform.position, 
                Vector2.down, groundCheckDistance, 
                groundMask);

            isWallDetected = Physics2D.Raycast(
                                 primaryWallCheck.transform.position,
                                 Vector2.right * Entity.entityMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask) &&
                             Physics2D.Raycast(
                                 secondaryWallCheck.transform.position,
                                 Vector2.right * Entity.entityMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask);
        }
        
        protected virtual void OnDrawGizmos()
        {
            const float wireSphereRadius = 0.05f;
            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(primaryWallCheck.position + Vector3.down * groundCheckDistance, wireSphereRadius);
            
            if (Entity == null) Entity = GetComponent<EntityController>();
            if (Entity == null || Entity.entityMove == null) return;
            if (primaryWallCheck == null || secondaryWallCheck == null) return;

            Gizmos.color = isWallDetected ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryWallCheck.transform.position, primaryWallCheck.transform.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(primaryWallCheck.transform.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir, wireSphereRadius);
            Gizmos.DrawLine(secondaryWallCheck.transform.position, secondaryWallCheck.transform.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(secondaryWallCheck.transform.position + Vector3.right * wallCheckDistance * Entity.entityMove.FacingDir, wireSphereRadius);
        }
    }
}