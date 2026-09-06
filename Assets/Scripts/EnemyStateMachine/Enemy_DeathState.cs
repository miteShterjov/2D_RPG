using Blueprints;
using EnemyControl;
using UnityEngine;

namespace EnemyStateMachine
{
    public class Enemy_DeathState : EnemyState
    {
        public Enemy_DeathState(
            EnemyController enemy, 
            StateMachine stateMachine, 
            string animBoolName) : base(enemy, stateMachine, animBoolName)
        {}

        private float deathBounceForce = 18;

        public override void Enter()
        {
            Anim.enabled = false;
            Rb.gravityScale = 12;
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, deathBounceForce);
            
            Enemy.GetComponent<Collider2D>().enabled = false;
            
            StateMachine.SwichOffStateMachine();
        }
    }
}