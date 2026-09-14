using EnemyControl;
using EntityStateMachine;

namespace EnemyStateMachine
{
    public class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Update()
        {
            base.Update();
            if (Enemy.enemyCollision.isPlayerDetected)
                Enemy.TryEnterBattleState(Enemy.enemyCollision.DetectedPlayer);
        }
    }
}