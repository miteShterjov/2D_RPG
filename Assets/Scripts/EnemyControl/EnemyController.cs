using EnemyStateMachine;
using EntityControl;
using EntityStateMachine;
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

        public EnemyIdleState IdleState;
        public EnemyMoveState MoveState;
        public EnemyAttackState AttackState;
        public EnemyBattleState BattleState;
        protected EnemyDeathState DeathState;
        protected EnemyStunnedState StunnedState;

        public bool IsAttackStateActive => StateMachine != null && StateMachine.CurrentState == AttackState;
        
        public EnemyMoveController enemyMove;
        public EnemyCollisionController enemyCollision;
        public EnemyCombatController enemyCombat;
        
        public Transform Player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            InitEnemyControllers();
        }

        protected override void Update()
        {
            base.Update();
            StateMachine.UpdateActiveState();
        }

        private void OnEnable() => PlayerController.OnPlayerDeath += HandlePlayerDeath;
        private void OnDisable() => PlayerController.OnPlayerDeath -= HandlePlayerDeath;
        
        public void EnableCounterWindow(bool enable) => cabBeStunned = enable;

        public override void EntityDeath()
        {
            base.EntityDeath();
            StateMachine.ChangeState(DeathState);
        }

        public void TryEnterBattleState(Transform player)
        {
            if (StateMachine.CurrentState == BattleState) return;
            if (StateMachine.CurrentState == AttackState) return;
            
            this.Player = player;
            StateMachine.ChangeState(BattleState);
        }

        private void InitEnemyControllers()
        {
            enemyMove = GetComponent<EnemyMoveController>();
            enemyCollision = GetComponent<EnemyCollisionController>();
            enemyCombat = GetComponent<EnemyCombatController>();
        }

        private void HandlePlayerDeath()
        {
            Player = null;
            if (StateMachine.CurrentState == DeathState || StateMachine.CurrentState == IdleState) return;
            StateMachine.ChangeState(IdleState);
        }
    }
}