using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(
            PlayerController player,
            StateMachine stateMachine, 
            string stateName) : base(player, stateMachine, stateName)
        {}
        
        private PlayerMoveController PlayerMove => Player.playerMove;

        private const float NoInputGracePeriod = 0.08f;
        private float _noInputTimer;

        public override void Enter()
        {
            base.Enter();
            _noInputTimer = 0f;
        }

        public override void Update()
        {
            base.Update();

            if (PlayerMove.MoveInput.x == 0)
            {
                _noInputTimer += Time.deltaTime;
                if (_noInputTimer >= NoInputGracePeriod)
                {
                    StateMachine.ChangeState(Player.IdleState);
                    return;
                }
            }
            else
            {
                _noInputTimer = 0f;
            }
            
            if (PlayerMove.MoveInput.x == 0 || Player.playerCollision.IsWallDetected)
                StateMachine.ChangeState(Player.IdleState);

            PlayerMove.SetVelocity(PlayerMove.MoveInput.x * PlayerMove.moveSpeed, Rb.linearVelocity.y);
        }
    }
}