using Blueprints;
using EnemyControl;
using EntityStateMachine;
using UnityEngine;

namespace EnemyStateMachine
{
    public class EnemyState : EntityState
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int MoveAnimSpeedMultyplier = Animator.StringToHash("moveAnimSpeedMultyplier");
        private static readonly int battleAnimSpeedMultiplier = Animator.StringToHash("battleAnimSpeedMultyplier");
        
        protected EnemyController Enemy;
        
        public EnemyState(
            EnemyController enemy,
            StateMachine stateMachine,
            string animBoolName) : base(stateMachine, animBoolName)
        {
            this.Enemy = enemy;
            Rb = enemy.Rb;
            Anim = enemy.Animator;
        }
        
        public override void UpdateAnimationParams()
        {
            base.UpdateAnimationParams();
            
            float battleAnimSpeedMultipler = Enemy.battleMoveSpeed / Enemy.enemyMove.moveSpeed;
            
            Anim.SetFloat(battleAnimSpeedMultiplier, battleAnimSpeedMultipler);
            Anim.SetFloat(MoveAnimSpeedMultyplier, Enemy.enemyMove.moveAnimSpeedMultyplier);
            Anim.SetFloat(XVelocity, Rb.linearVelocity.x);
        }
    }
}