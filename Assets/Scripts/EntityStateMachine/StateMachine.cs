namespace EntityStateMachine
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set;}
        
        private bool isLockedState;
    
        public void Initialize(EntityState startState)
        {
            if (startState == null) return;

            CurrentState = startState;
            CurrentState.Enter();
        }
    
        public void ChangeState(EntityState newState)
        {
            if (isLockedState || newState == null) return;
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    
        public void UpdateActiveState() => CurrentState.Update();
        public void LockOffStateMachine() => isLockedState = true;
    }
}
