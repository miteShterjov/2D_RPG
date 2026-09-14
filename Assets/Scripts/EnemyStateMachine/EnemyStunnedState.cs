using EnemyControl;
using EntityStateMachine;
using UnityEngine;

namespace EnemyStateMachine
{
    public class EnemyStunnedState : EnemyState
    {
        public EnemyStunnedState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
        
            Enemy.GetComponent<EnemyVFX>().EnableAttackAlert(false);
            Enemy.EnableCounterWindow(false);
        
            stateTimer = Enemy.stunnedDuration;
            Rb.linearVelocity = new Vector2(Enemy.stunnedVelocity.x * -Enemy.enemyMove.FacingDir, Enemy.stunnedVelocity.y);
        }

        public override void Update()
        {
            base.Update();
            if (stateTimer < 0) StateMachine.ChangeState(Enemy.IdleState);
        }
    }
}
