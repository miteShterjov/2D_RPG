using System;
using Blueprints;
using EnemyStateMachine;
using EntityControl;
using PlayerControl;
using UnityEngine;

namespace EnemyControl
{
    public class EnemyController : EntityController
    {
        [Header("Battle Config")] [SerializeField]
        public float battleMoveSpeed = 3f;
        [SerializeField] public float attackDistance = 2f;
        [SerializeField] public float battleTimeDuration = 5f;
        [SerializeField] public float minRetreatDistance = 2f;
        [SerializeField] public Vector2 retreatVelocity;
        [Header("Stunned Config")] 
        [SerializeField] public float stunnedDuration = 1f;
        [SerializeField] public Vector2 stunnedVelocity = new Vector2(3f, 3f);
        public bool cabBeStunned;

        public Enemy_IdleState IdleState;
        public Enemy_MoveState MoveState;
        public Enemy_AttackState AttackState;
        public Enemy_BattleState BattleState;
        public Enemy_DeathState DeathState;
        public Enemy_StunnedState StunnedState;

        public bool IsAttackStateActive => _stateMachine != null && _stateMachine.CurrentState == AttackState;
        
        public EnemyMoveController enemyMove;
        public EnemyCollisionController enemyCollision;
        public EnemyCombatController enemyCombat;
        
        public Transform player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine();
            InitEnemyControllers();
        }

        protected override void Update()
        {
            base.Update();
            _stateMachine.UpdateActiveState();
        }

        private void OnEnable() => PlayerController.OnPlayerDeath += HandlePlayerDeath;
        private void OnDisable() => PlayerController.OnPlayerDeath -= HandlePlayerDeath;
        
        public void EnableCounterWindow(bool enable) => cabBeStunned = enable;

        public override void EntityDeath()
        {
            base.EntityDeath();
            _stateMachine.ChangeState(DeathState);
        }

        public void TryEnterBattleState(Transform player)
        {
            if (_stateMachine.CurrentState == BattleState) return;
            if (_stateMachine.CurrentState == AttackState) return;
            this.player = player;
            _stateMachine.ChangeState(BattleState);
        }

        private void InitEnemyControllers()
        {
            enemyMove = GetComponent<EnemyMoveController>();
            enemyCollision = GetComponent<EnemyCollisionController>();
            enemyCombat = GetComponent<EnemyCombatController>();
        }

        private void HandlePlayerDeath()
        {
            player = null;
            if (_stateMachine.CurrentState == DeathState || _stateMachine.CurrentState == IdleState) return;
            _stateMachine.ChangeState(IdleState);
        }
    }
}