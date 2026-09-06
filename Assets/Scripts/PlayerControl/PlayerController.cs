using System;
using System.Collections;
using Blueprints;
using EntityControl;
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
        public Player_DeathState DeathState;
        public Player_CounterAttack CounterAttack;

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

            _stateMachine = new StateMachine();

            InitPlayerStates();
            InitPlayerControllers();
        }

        protected void Start()
        {
            _stateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();
            _stateMachine.UpdateActiveState();
        }
        
        public override void EntityDeath()
        {
            base.EntityDeath();
            OnPlayerDeath?.Invoke();
            _stateMachine.ChangeState(DeathState);
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
        
        private void InitPlayerControllers()
        {
            playerMove = GetComponent<PlayerMoveController>();
            playerCollision = GetComponent<PlayerCollisionController>();
            playerCombat = GetComponent<PlayerCombatController>();
        }
        
        private void InitPlayerStates()
        {
            IdleState = new PlayerIdleState(this, _stateMachine, IdleAnimConst);
            MoveState = new PlayerMoveState(this, _stateMachine, MoveAnimConst);
            JumpState = new PlayerJumpState(this, _stateMachine, JumpFallAnimConst);
            FallState = new PlayerFallState(this, _stateMachine, JumpFallAnimConst);
            WallSlideState = new PlayerWallSlide(this, _stateMachine, WallSlideAnimConst);
            WallJump = new PlayerWallJump(this, _stateMachine, JumpFallAnimConst);
            SprintState = new PlayerSprintState(this, _stateMachine, SprintAnimConst);
            BasicAttackState = new PlayerBasicAttackState(this, _stateMachine, BasicAttackAnimConst);
            JumpAttackState = new PLayerJumpAttackState(this, _stateMachine, JumpAttackAnimConst);
            DeathState = new Player_DeathState(this, _stateMachine, DeathAnimConst);
            CounterAttack = new Player_CounterAttack(this, _stateMachine, CounterAttackAnimConst);
        }
    }
}