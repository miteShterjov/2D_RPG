using EnemyControl;
using EntityStateMachine;

namespace EnemyStateMachine
{
    public class EnemyIdleState : EnemyGroundedState
    {
        public EnemyIdleState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
            stateTimer = Enemy.enemyMove.patrolIdleTime;
        }

        public override void Update()
        {
            base.Update();
            if (StateMachine.CurrentState == Enemy.BattleState) return;
            if (stateTimer < 0) 
                StateMachine.ChangeState(Enemy.MoveState);
        }
    }
}