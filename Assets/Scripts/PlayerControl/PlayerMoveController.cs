using EntityControl;
using UI;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerMoveController : EntityMoveController
    {
        public Vector2 MoveInput { get; private set;}
        public Vector2 MousePosition { get; set;}
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
        
        private UIHandler uiHandler;

        private PlayerController playerController;

        protected override void Awake()
        {
            base.Awake();
            uiHandler = FindAnyObjectByType<UIHandler>();
            InputActions = new InputSystem_Actions();
            
            playerController = GetComponent<PlayerController>();
        }
        
        private void OnEnable()
        {
            InputActions.Enable();
            InputActions.Player.Move.performed += HandleMovePerformed;
            InputActions.Player.Move.canceled += HandleMoveCanceled;
            
            InputActions.Player.MouseAim.performed += ctx => 
                MousePosition = ctx.ReadValue<Vector2>();

            InputActions.UI.ToggleSkillTreeUI.performed += context => 
                uiHandler.ToggleSkillTree();

            InputActions.Player.SpellCast.performed += ctx => 
                playerController.skillsManager.Shard.TryUseSkill();
            
            InputActions.Player.SpellCast.performed += ctx => 
                playerController.skillsManager.TimeEcho.TryUseSkill();
        }
        
        private void OnDisable()
        {
            InputActions.Player.Move.performed -= HandleMovePerformed;
            InputActions.Player.Move.canceled -= HandleMoveCanceled;
            InputActions.Disable();
            MoveInput = Vector2.zero;
        }
        
        public void TeleportPlayer(Vector3 position) => transform.position = position;

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