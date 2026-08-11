using UnityEngine;

namespace PlayerControl
{
    public class PlayerMoveController : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set;}
        public InputSystem_Actions InputActions { get; private set; }
        public Rigidbody2D rb;
        public float JumpForce => jumpForce;
        public int FacingDir => facingDir;
        public float InAirMoveMultiplier => inAirMoveMultiplier;

        [Header("Locomotion")]
        [SerializeField] public float moveSpeed = 10f;
        [SerializeField] public float jumpForce = 5f;
        [SerializeField] public float sprintSpeed = 20f;
        [SerializeField] public float sprintDuration = 0.25f;
        [SerializeField] public Vector2 wallJumpAngle = new Vector2(6f, 12f);
        [SerializeField] [Range(0, 1)] private float inAirMoveMultiplier;
        [SerializeField] [Range(0, 1)] public float wallSlideSlowMultiplier;
        [SerializeField] private bool isFacingRight = true;
        [SerializeField] private int facingDir = 1;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            InputActions = new InputSystem_Actions();
        }
        
        private void OnEnable()
        {
            InputActions.Enable();
            InputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            InputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
        }
        
        private void OnDisable() => InputActions.Disable();
        
        public void SetVelocity(float x, float y)
        {
            rb.linearVelocity = new Vector2(x, y);
            HandleFlipPlayerSprite(x);
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            rb.linearVelocity = velocity;
            HandleFlipPlayerSprite(velocity.x);
        }

        public void FlipPlayerSprite()
        {
            transform.localScale = new Vector3(isFacingRight ? -1 : 1, 1, 1);
            isFacingRight = !isFacingRight;
            facingDir = isFacingRight ? 1 : -1;
        }
    
        private void HandleFlipPlayerSprite(float xVelocity)
        {
            if (xVelocity > 0 && !isFacingRight || xVelocity < 0 && isFacingRight) FlipPlayerSprite();
        }
    }
}