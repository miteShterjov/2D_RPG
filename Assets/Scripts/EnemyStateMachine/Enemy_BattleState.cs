using Blueprints;
using EnemyControl;
using UnityEngine;

namespace EnemyStateMachine
{
    public class Enemy_BattleState : EnemyState
    {
        public Enemy_BattleState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Battle State");
        }
    }
}
