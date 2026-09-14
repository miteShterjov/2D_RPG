using System;
using System.Collections;
using EntityControl;
using EntityStateMachine;
using PlayerStateMachine;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerController : EntityController
    {
        public static event Action OnPlayerDeath;
        
        [Header("Attack Details")] 
        public Vector2[] attackVelocity;
        public Vector2 jumpAttackVelocity;
        public float attackVelocityDuration = 0.1f;
        public float comboAttackResetTime = 1f;
        private Coroutine _queuedAttackCo;

        public PlayerMoveController playerMove;
        public PlayerCollisionController playerCollision;
        public PlayerCombatController playerCombat;

        public PlayerIdleState IdleState;
        public PlayerMoveState MoveState;
        public PlayerJumpState JumpState;
        public PlayerFallState FallState;
        public PlayerWallSlide WallSlideState;
        public PlayerWallJump WallJump;
        public PlayerSprintState SprintState;
        public PlayerBasicAttackState BasicAttackState;
        public PLayerJumpAttackState JumpAttackState;
        public PlayerCounterAttack CounterAttack;
        private PlayerDeathState deathState;

        private const string IdleAnimConst = "idle";
        private const string MoveAnimConst = "move";
        private const string JumpFallAnimConst = "jumpFall";
        private const string WallSlideAnimConst = "wallSlide";
        private const string SprintAnimConst = "sprint";
        private const string BasicAttackAnimConst = "basicAttack";
        private const string JumpAttackAnimConst = "jumpAttack";
        private const string DeathAnimConst = "death";
        private const string CounterAttackAnimConst = "counterAttack";
        
        protected override void Awake()
        {
            base.Awake();

            StateMachine = new StateMachine();

            InitPlayerStates();
            InitPlayerControllers();
        }

        protected void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        protected void OnDisable()
        {
            if (_queuedAttackCo != null)
            {
                StopCoroutine(_queuedAttackCo);
                _queuedAttackCo = null;
            }
        }

        protected override void Update()
        {
            base.Update();
            StateMachine.UpdateActiveState();
        }
        
        public override void EntityDeath()
        {
            base.EntityDeath();
            OnPlayerDeath?.Invoke();
            StateMachine.ChangeState(deathState);
        }

        public void EnterAttackStateWithDelay()
        {
            if (_queuedAttackCo != null) StopCoroutine(_queuedAttackCo);
            _queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
        }

        private IEnumerator EnterAttackStateWithDelayCo()
        {
            yield return new WaitForEndOfFrame();
            _queuedAttackCo = null;
            StateMachine.ChangeState(BasicAttackState);
        }
        
        private void InitPlayerControllers()
        {
            playerMove = GetComponent<PlayerMoveController>();
            playerCollision = GetComponent<PlayerCollisionController>();
            playerCombat = GetComponent<PlayerCombatController>();
        }
        
        private void InitPlayerStates()
        {
            IdleState = new PlayerIdleState(this, StateMachine, IdleAnimConst);
            MoveState = new PlayerMoveState(this, StateMachine, MoveAnimConst);
            JumpState = new PlayerJumpState(this, StateMachine, JumpFallAnimConst);
            FallState = new PlayerFallState(this, StateMachine, JumpFallAnimConst);
            WallSlideState = new PlayerWallSlide(this, StateMachine, WallSlideAnimConst);
            WallJump = new PlayerWallJump(this, StateMachine, JumpFallAnimConst);
            SprintState = new PlayerSprintState(this, StateMachine, SprintAnimConst);
            BasicAttackState = new PlayerBasicAttackState(this, StateMachine, BasicAttackAnimConst);
            JumpAttackState = new PLayerJumpAttackState(this, StateMachine, JumpAttackAnimConst);
            deathState = new PlayerDeathState(this, StateMachine, DeathAnimConst);
            CounterAttack = new PlayerCounterAttack(this, StateMachine, CounterAttackAnimConst);
        }
    }
}