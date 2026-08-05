using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(
        PlayerController player,
        StateMachine stateMachine, 
        string stateName) : base(player, stateMachine, stateName)
    {}

    public override void Enter()
    {
        Debug.Log("Player enters idle state");
    }
        
    public override void Update()
    {
        if (player.MoveInput.x != 0) stateMachine.ChangeState(player.MoveState);
    }
        
    public override void Exit()
    {
        Debug.Log("Player leaves idle state");
    }
}