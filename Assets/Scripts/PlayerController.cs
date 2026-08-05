using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player_IdleState IdleState;
    public Player_MoveState MoveState;
    
    public Vector2 MoveInput { get; private set;}
        
    private InputSystem_Actions _inputActions;
    private StateMachine _stateMachine;
        
        

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _inputActions = new InputSystem_Actions();
            
        IdleState = new Player_IdleState(this, _stateMachine, "idle");
        MoveState = new Player_MoveState(this, _stateMachine, "move");
    }
        
    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }
        
    private void OnDisable() => _inputActions.Disable();
        
    private void Start()
    {
        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        _stateMachine.UpdateActiveState();
    }
}