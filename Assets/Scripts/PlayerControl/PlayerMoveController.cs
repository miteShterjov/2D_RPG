using EntityControl;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerMoveController : EntityMoveController
    {
        public Vector2 MoveInput { get; private set;}
        public InputSystem_Actions InputActions { get; private set; }
        public float JumpForce => jumpForce;
        public float InAirMoveMultiplier => inAirMoveMultiplier;
        
        [Header("Movement Config")]
        [SerializeField] public float jumpForce = 5f;
        [SerializeField] public float sprintSpeed = 20f;
        [SerializeField] public float sprintDuration = 0.25f;
        [SerializeField] public Vector2 wallJumpAngle = new Vector2(6f, 12f);
        [SerializeField] [Range(0, 1)] private float inAirMoveMultiplier;
        [SerializeField] [Range(0, 1)] public float wallSlideSlowMultiplier;

        protected override void Awake()
        {
            base.Awake();
            InputActions = new InputSystem_Actions();
        }
        
        private void OnEnable()
        {
            InputActions.Enable();
            InputActions.Player.Move.performed += HandleMovePerformed;
            InputActions.Player.Move.canceled += HandleMoveCanceled;
        }
        
        private void OnDisable()
        {
            InputActions.Player.Move.performed -= HandleMovePerformed;
            InputActions.Player.Move.canceled -= HandleMoveCanceled;
            InputActions.Disable();
            MoveInput = Vector2.zero;
        }

        private void HandleMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void HandleMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
        }
    }
}