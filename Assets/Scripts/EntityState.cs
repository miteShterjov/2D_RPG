using DefaultNamespace;
using UnityEngine;

public abstract class EntityState
{
    protected PlayerController player;
    protected StateMachine stateMachine;
    protected string stateName;

    public EntityState(
        PlayerController player, 
        StateMachine stateMachine, 
        string stateName
        )
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }
    
    // first point when entering the state
    public virtual void Enter() {}
    
    // this is where the logic of the state is implemented
    public virtual void Update() {}
    
    // called when we leave the state
    public virtual void Exit() {}
    
    
}
