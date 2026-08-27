using EntityControl;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyControl
{
    public class EnemyCollisionController : EntityCollisionController
    {
        [SerializeField] protected Transform primaryGroundCheck;
        [SerializeField] protected float aggroRange = 10f;
        [SerializeField] protected LayerMask playerMask;
        
        public bool isPlayerDetected;

        protected override void Update()
        {
            HandleGroundCollision();
            HandleWallCollision();
            HandlePlayerCollision();
        }

        private void HandleGroundCollision()
        {
            isGrounded = Physics2D.Raycast(
                primaryGroundCheck.position,
                Vector2.down, groundCheckDistance,
                groundMask);
        }

        private void HandleWallCollision()
        {
            isWallDetected = Physics2D.Raycast(
                                 primaryWallCheck.transform.position,
                                 Vector2.right * _entity.entityMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask) 
                             &&
                             Physics2D.Raycast(
                                 secondaryWallCheck.transform.position,
                                 Vector2.right * _entity.entityMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask);
        }

        private void HandlePlayerCollision()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                Vector2.right * _entity.entityMove.FacingDir,
                aggroRange,
                playerMask | groundMask);
    
            // only detected if the FIRST thing hit is the player, not a wall in between
            isPlayerDetected = hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }
        
        
        protected override void OnDrawGizmos()
        {
            if (_entity == null) _entity = GetComponent<EntityController>();
            if (_entity == null || _entity.entityMove == null) return;
            if (primaryGroundCheck == null || primaryWallCheck == null || secondaryWallCheck == null) return;
    
            const float wireSphereRadius = 0.09f;
    
            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawLine(
                primaryGroundCheck.position, 
                primaryGroundCheck.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(primaryGroundCheck.position + Vector3.down * groundCheckDistance, wireSphereRadius);
    
            Gizmos.color = isWallDetected ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + Vector3.right * wallCheckDistance * _entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(primaryWallCheck.position + Vector3.right * wallCheckDistance * _entity.entityMove.FacingDir, wireSphereRadius);
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + Vector3.right * wallCheckDistance * _entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(secondaryWallCheck.position + Vector3.right * wallCheckDistance * _entity.entityMove.FacingDir, wireSphereRadius);
            
            Gizmos.color = isPlayerDetected ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * aggroRange * _entity.entityMove.FacingDir);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * aggroRange * _entity.entityMove.FacingDir, wireSphereRadius);
        }
    }
}
