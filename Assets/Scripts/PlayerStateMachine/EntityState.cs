using Blueprints;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public abstract class EntityState
    {
        public readonly Animator _anim;
        protected readonly Rigidbody2D Rb;
    
        protected readonly PlayerController Player;
        protected readonly StateMachine StateMachine;
        protected readonly string AnimBoolName;

        protected float StateTimer;
        protected bool TriggerCalled;
    
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");

        protected EntityState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName
        )
        {
            this.Player = player;
            this.StateMachine = stateMachine;
            this.AnimBoolName = animBoolName;
        
            _anim = player.Animator;
            Rb = player.playerMove.rb;
        }
    
        // first point when entering the state
        public virtual void Enter()
        {
            _anim.SetBool(AnimBoolName, true);
            TriggerCalled = false;
        }
    
        // this is where the logic of the state is implemented
        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
            
            _anim.SetFloat(YVelocity, Rb.linearVelocity.y);
            
            if (Player.playerMove.InputActions.Player.Sprint.WasPressedThisFrame() && CanSprint())
                StateMachine.ChangeState(Player.SprintState);
        }
    
        // called when we leave the state
        public virtual void Exit()
        {
            _anim.SetBool(AnimBoolName, false);
        }
        
        public void CallAnimTrigger() => TriggerCalled = true;

        private bool CanSprint()
        {
            if (Player.playerCollision.IsWallDetected) return false;
            if (StateMachine.CurrentState == Player.SprintState) return false;
            return true;
        }
    }
}
