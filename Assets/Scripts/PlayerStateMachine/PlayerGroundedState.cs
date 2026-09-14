using EntityStateMachine;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();
            
            if (Rb.linearVelocity.y < 0 && !Player.playerCollision.IsGrounded) 
                StateMachine.ChangeState(Player.FallState);
            
            if (Player.playerMove.InputActions.Player.Jump.WasPressedThisFrame()) 
                StateMachine.ChangeState(Player.JumpState);
            
            if (Player.playerMove.InputActions.Player.Attack.WasPressedThisFrame())
                StateMachine.ChangeState(Player.BasicAttackState);
            
            if (Player.playerMove.InputActions.Player.CounterAttack.WasPressedThisFrame())
                StateMachine.ChangeState(Player.CounterAttack);
        }
    }
}