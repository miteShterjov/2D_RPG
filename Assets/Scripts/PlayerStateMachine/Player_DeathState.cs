using Blueprints;
using PlayerControl;
using UnityEngine;

namespace PlayerStateMachine
{
    public class Player_DeathState : PlayerState
    {
        public Player_DeathState(
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