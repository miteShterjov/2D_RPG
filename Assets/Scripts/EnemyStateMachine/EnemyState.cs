using EnemyControl;
using EntityStateMachine;
using UnityEngine;

namespace EnemyStateMachine
{
    public class EnemyState : EntityState
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int MoveAnimSpeedMultiplier = Animator.StringToHash("moveAnimSpeedMultiplier");
        private static readonly int BattleAnimSpeedMultiplier = Animator.StringToHash("battleAnimSpeedMultiplier");
        
        protected readonly EnemyController Enemy;

        protected EnemyState(
            EnemyController enemy,
            StateMachine stateMachine,
            string animBoolName) : base(stateMachine, animBoolName)
        {
            this.Enemy = enemy;
            Rb = enemy.Rb;
            Anim = enemy.Animator;
        }

        protected override void UpdateAnimationParams()
        {
            base.UpdateAnimationParams();
            
            float battleAnimSpeedMultiplier = Enemy.enemyMove.moveSpeed == 0f
                ? 0f
                : Enemy.battleMoveSpeed / Enemy.enemyMove.moveSpeed;
            
            Anim.SetFloat(EnemyState.BattleAnimSpeedMultiplier, battleAnimSpeedMultiplier);
            Anim.SetFloat(MoveAnimSpeedMultiplier, Enemy.enemyMove.moveAnimSpeedMultiplier);
            Anim.SetFloat(XVelocity, Rb.linearVelocity.x);
        }
    }
}