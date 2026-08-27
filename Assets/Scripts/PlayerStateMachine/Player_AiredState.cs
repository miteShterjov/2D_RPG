using Blueprints;
using PlayerControl;

namespace PlayerStateMachine
{
    public class PlayerAiredState : PlayerState
    {
        protected PlayerAiredState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        { }

        private PlayerMoveController PlayerMove => Player.playerMove;
        
        public override void Update()
        {
            base.Update();

            if (PlayerMove.MoveInput.x != 0) 
                PlayerMove.SetVelocity(
                    PlayerMove.MoveInput.x * (PlayerMove.moveSpeed * PlayerMove.InAirMoveMultiplier), 
                    Rb.linearVelocity.y);
            
            if (Player.playerMove.InputActions.Player.Attack.WasPressedThisFrame())
                StateMachine.ChangeState(Player.JumpAttackState);
        }
    }
}
