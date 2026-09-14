using EnemyControl;
using EntityStateMachine;
using UnityEngine;

namespace EnemyStateMachine
{
    public class EnemyDeathState : EnemyState
    {
        public EnemyDeathState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        {}

        private const float DeathBounceForce = 18;

        public override void Enter()
        {
            Anim.enabled = false;
            Rb.gravityScale = 12;
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, DeathBounceForce);
            
            Enemy.GetComponent<Collider2D>().enabled = false;
            
            StateMachine.LockOffStateMachine();
        }
    }
}