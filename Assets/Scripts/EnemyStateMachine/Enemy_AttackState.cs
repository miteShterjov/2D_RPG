using Blueprints;
using EnemyControl;
using UnityEngine;

namespace EnemyStateMachine
{
    public class Enemy_AttackState : EnemyState
    {
        public Enemy_AttackState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Update()
        {
            base.Update();
            if(TriggerCalled) StateMachine.ChangeState(Enemy.IdleState);
        }
    }
}