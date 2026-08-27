using Blueprints;
using EnemyControl;
using EnemyStateMachine;

namespace EnemyNPCs
{
    public class EnemySkeleton : EnemyController
    {
        private const string IdleAnimConst = "idle";
        private const string MoveAnimConst = "move";
        private const string AttackAnimConst = "attack";
        private const string BattleAnimConst = "battle";

        protected override void Awake()
        {
            base.Awake();

            IdleState = new Enemy_IdleState(this, _stateMachine, IdleAnimConst);
            MoveState = new Enemy_MoveState(this, _stateMachine, MoveAnimConst);
            AttackState = new Enemy_AttackState(this, _stateMachine, AttackAnimConst);
            BattleState = new Enemy_BattleState(this, _stateMachine, BattleAnimConst);
        }
        
        protected void Start()
        {
            _stateMachine.Initialize(IdleState);
        }
    }
}
