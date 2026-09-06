using Blueprints;
using EnemyControl;
using EntityStateMachine;
using UnityEngine;

namespace EnemyStateMachine
{
    public class Enemy_GroundedState : EnemyState
    {
        public Enemy_GroundedState(
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