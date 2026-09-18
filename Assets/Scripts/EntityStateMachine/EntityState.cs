using EntityStats;
using UnityEngine;

namespace EntityStateMachine
{
    public abstract class EntityState
    {
        protected readonly StateMachine StateMachine;
        protected readonly string AnimBoolName;

        protected Animator Anim;
        protected Rigidbody2D Rb;
        protected GeneralStats generalStats;

        protected float stateTimer;
        protected bool TriggerCalled;
        
        private static readonly int AttackSpeedMultiplier = Animator.StringToHash("attackSpeedMultiplier");

        protected EntityState(StateMachine stateMachine, string animBoolName)
        {
            this.StateMachine = stateMachine;
            this.AnimBoolName = animBoolName;
        }
        
        // first point when entering the state
        public virtual void Enter()
        {
            Anim.SetBool(AnimBoolName, true);
            TriggerCalled = false;
        }
    
        // this is where the logic of the state is implemented
        public virtual void Update()
        {
            stateTimer -= Time.deltaTime;
            UpdateAnimationParams();
        }
    
        // called when we leave the state
        public virtual void Exit()
        {
            Anim.SetBool(AnimBoolName, false);
        }
        
        public void CallAnimTrigger() => TriggerCalled = true;

        protected virtual void UpdateAnimationParams() {}

        protected void SyncAttackSpeed()
        {
            float attackSpeed = generalStats.offenseStats.attackSpeed.GetValue();
            Anim.SetFloat(AttackSpeedMultiplier, attackSpeed);
        }
    }
}