using UnityEngine;

namespace DefaultNamespace
{
    public class Player_MoveState : EntityState
    {
        public Player_MoveState(
            PlayerController player,
            StateMachine stateMachine, 
            string stateName) : base(player, stateMachine, stateName)
        {}
        
        public override void Enter()
        {
            Debug.Log("Player enters move state");
        }
        
        public override void Update()
        {
            if (player.MoveInput.x == 0) stateMachine.ChangeState(player.IdleState);
        }
        
        public override void Exit()
        {
            Debug.Log("Player leaves move state");
        }
    }
}