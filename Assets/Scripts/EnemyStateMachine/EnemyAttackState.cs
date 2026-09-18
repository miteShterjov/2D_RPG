using EnemyControl;
using EntityStateMachine;

namespace EnemyStateMachine
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
            SyncAttackSpeed();
        }

        public override void Update()
        {
            base.Update();
            if(TriggerCalled) StateMachine.ChangeState(Enemy.BattleState);
        }
    }
}