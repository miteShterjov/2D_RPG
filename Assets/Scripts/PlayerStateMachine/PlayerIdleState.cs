using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(
            PlayerController player,
            StateMachine stateMachine,
            string stateName) : base(player, stateMachine, stateName)
        {
        }
        
        private PlayerMoveController PlayerMove => Player.playerMove;

        public override void Enter()
        {
            base.Enter();
            Player.playerMove.SetVelocity(0, Rb.linearVelocity.y);
        }

        public override void Update()
        {
            base.Update();

            if (Mathf.Approximately(PlayerMove.MoveInput.x, PlayerMove.FacingDir) && Player.playerCollision.IsWallDetected)
                return;
            
            if (Player.playerMove.MoveInput.x != 0) StateMachine.ChangeState(Player.MoveState);
        }
    }
}