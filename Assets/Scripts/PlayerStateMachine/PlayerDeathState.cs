using EntityStateMachine;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(
            PlayerController player, 
            StateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        {}

        public override void Enter()
        {
            base.Enter();
            Player.playerMove.InputActions.Disable();
            //Player.GetComponent<Collider2D>().enabled = false;
        }
    }
}