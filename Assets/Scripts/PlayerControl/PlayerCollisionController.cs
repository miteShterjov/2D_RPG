using UnityEngine;

namespace PlayerControl
{
    public class PlayerCollisionController : MonoBehaviour
    {
        public bool IsGrounded => isGrounded;
        public bool IsWallDetected => isWallDetected;
        
        [Header("Collision")]
        [SerializeField] private float groundCheckDistance = 1.4f;
        [SerializeField] private float wallCheckDistance = 1.4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isWallDetected;
        [SerializeField] private Transform primaryWallCheck;
        [SerializeField] private Transform secondaryWallCheck;

        private PlayerController _player;

        private void Start()
        {
            _player = GetComponent<PlayerController>();
        }

        private void Update()
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
                                 Vector2.right * _player.playerMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask) &&
                             Physics2D.Raycast(
                                 secondaryWallCheck.transform.position,
                                 Vector2.right * _player.playerMove.FacingDir,
                                 wallCheckDistance,
                                 groundMask);


        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);

            if (_player == null) _player = GetComponent<PlayerController>();
            if (_player == null || _player.playerMove == null) return;

            Gizmos.color = isWallDetected ? Color.green : Color.yellow;
            Gizmos.DrawLine(primaryWallCheck.transform.position, primaryWallCheck.transform.position + Vector3.right * wallCheckDistance * _player.playerMove.FacingDir);
            Gizmos.DrawLine(secondaryWallCheck.transform.position, secondaryWallCheck.transform.position + Vector3.right * wallCheckDistance * _player.playerMove.FacingDir);
        }
    }
}