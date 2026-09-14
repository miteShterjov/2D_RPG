using EntityStateMachine;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerJumpState : PlayerAiredState
    {
        public PlayerJumpState(
            PlayerController player,
            StateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.playerMove.SetVelocity(
                Rb.linearVelocity.x, 
                Player.playerMove.JumpForce);
        }

        public override void Update()
        {
            base.Update();
            if (Rb.linearVelocity.y < 0 && StateMachine.CurrentState != Player.JumpAttackState) 
                StateMachine.ChangeState(Player.FallState);
        }
    }
}