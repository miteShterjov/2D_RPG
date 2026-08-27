using System.Collections;
using Blueprints;
using EntityControl;
using PlayerStateMachine;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerController : EntityController
    {
        [Header("Attack Details")] 
        public Vector2[] attackVelocity;
        public Vector2 jumpAttackVelocity;
        public float attackVelocityDuration = 0.1f;
        public float comboAttackResetTime = 1f;
        private Coroutine _queuedAttackCo;

        public PlayerMoveController playerMove;
        public PlayerCollisionController playerCollision;

        public PlayerIdleState IdleState;
        public PlayerMoveState MoveState;
        public PlayerJumpState JumpState;
        public PlayerFallState FallState;
        public PlayerWallSlide WallSlideState;
        public PlayerWallJump WallJump;
        public PlayerSprintState SprintState;
        public PlayerBasicAttackState BasicAttackState;
        public PLayerJumpAttackState JumpAttackState;

        private const string IdleAnimConst = "idle";
        private const string MoveAnimConst = "move";
        private const string JumpFallAnimConst = "jumpFall";
        private const string WallSlideAnimConst = "wallSlide";
        private const string SprintAnimConst = "sprint";
        private const string BasicAttackAnimConst = "basicAttack";
        private const string JumpAttackAnimConst = "jumpAttack";
        
        protected override void Awake()
        {
            base.Awake();

            _stateMachine = new StateMachine();

            IdleState = new PlayerIdleState(this, _stateMachine, IdleAnimConst);
            MoveState = new PlayerMoveState(this, _stateMachine, MoveAnimConst);
            JumpState = new PlayerJumpState(this, _stateMachine, JumpFallAnimConst);
            FallState = new PlayerFallState(this, _stateMachine, JumpFallAnimConst);
            WallSlideState = new PlayerWallSlide(this, _stateMachine, WallSlideAnimConst);
            WallJump = new PlayerWallJump(this, _stateMachine, JumpFallAnimConst);
            SprintState = new PlayerSprintState(this, _stateMachine, SprintAnimConst);
            BasicAttackState = new PlayerBasicAttackState(this, _stateMachine, BasicAttackAnimConst);
            JumpAttackState = new PLayerJumpAttackState(this, _stateMachine, JumpAttackAnimConst);
        }

        private void Start()
        {
            _stateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            _stateMachine.UpdateActiveState();
        }

        public void EnterAttackStateWithDelay()
        {
            if (_queuedAttackCo != null) StopCoroutine(_queuedAttackCo);
            _queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
        }

        private IEnumerator EnterAttackStateWithDelayCo()
        {
            yield return new WaitForEndOfFrame();
            _stateMachine.ChangeState(BasicAttackState);
        }
    }
}