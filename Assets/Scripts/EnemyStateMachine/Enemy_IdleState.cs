using Blueprints;
using EnemyControl;

namespace EnemyStateMachine
{
    public class Enemy_IdleState : Enemy_GroundedState
    {
        public Enemy_IdleState(
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