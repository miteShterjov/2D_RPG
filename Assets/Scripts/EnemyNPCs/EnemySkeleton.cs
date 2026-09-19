using EnemyControl;
using EnemyStateMachine;
using Interface;

namespace EnemyNPCs
{
    public class EnemySkeleton : EnemyController, ICounterable
    {
        
        public bool CanBeCountered => cabBeStunned;

        private const string IdleAnimConst = "idle";
        private const string MoveAnimConst = "move";
        private const string AttackAnimConst = "attack";
        private const string BattleAnimConst = "battle";
        private const string StunnedAnimConst = "stun";

        protected override void Awake()
        {
            base.Awake();
            InitEnemyStates();
        }

        protected void Start() => StateMachine.ChangeState(IdleState);
        
        private void InitEnemyStates()
        {
            IdleState = new EnemyIdleState(this, StateMachine, IdleAnimConst);
            MoveState = new EnemyMoveState(this, StateMachine, MoveAnimConst);
            AttackState = new EnemyAttackState(this, StateMachine, AttackAnimConst);
            BattleState = new EnemyBattleState(this, StateMachine, BattleAnimConst);
            DeathState = new EnemyDeathState(this, StateMachine, IdleAnimConst);
            StunnedState = new EnemyStunnedState(this, StateMachine, StunnedAnimConst);
        }

        public void HandleCounterAttack()
        {
            if (!CanBeCountered || StateMachine.CurrentState != AttackState) return;
            
            EnableCounterWindow(false);
            StateMachine.ChangeState(StunnedState);
        } 
    }
}
