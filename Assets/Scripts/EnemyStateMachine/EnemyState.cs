using Blueprints;
using EnemyControl;
using EntityStateMachine;
namespace EnemyStateMachine
{
    public class EnemyState : EntityState
    {
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
        
        
    }
}