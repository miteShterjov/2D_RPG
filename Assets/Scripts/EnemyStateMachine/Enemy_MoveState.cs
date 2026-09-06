using Blueprints;
using EnemyControl;
using UnityEngine;

namespace EnemyStateMachine
{
    public class Enemy_MoveState : Enemy_GroundedState
    {
        public Enemy_MoveState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        { }

        public override void Enter()
        {
            base.Enter();
            if (!Enemy.enemyCollision.IsGrounded || Enemy.enemyCollision.IsWallDetected) Enemy.enemyMove.FlipEntitySprite();
        }

        public override void Update()
        {
            base.Update();
            if (StateMachine.CurrentState == Enemy.BattleState) return;
            
            Enemy.entityMove.SetVelocity(
                Enemy.enemyMove.moveSpeed * Enemy.enemyMove.FacingDir,
                Enemy.Rb.linearVelocity.y
            );

            if (!Enemy.entityCollision.IsGrounded || Enemy.enemyCollision.IsWallDetected)
            {
                Enemy.enemyMove.SetVelocity(Vector3.zero.x, Rb.linearVelocity.y);
                StateMachine.ChangeState(Enemy.IdleState);
            }
        }
    }
}