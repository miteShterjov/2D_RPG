using EntityStateMachine;

namespace Blueprints
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set;}
        // Once entered in isLockedState=true State no exit from it. Example death state. 
        public bool isLockedState = false;
    
        public void Initialize(EntityState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }
    
        public void ChangeState(EntityState newState)
        {
            if (isLockedState) return;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    
        public void UpdateActiveState() => CurrentState.Update();
        public void SwichOffStateMachine() => isLockedState = true;
    }
}
