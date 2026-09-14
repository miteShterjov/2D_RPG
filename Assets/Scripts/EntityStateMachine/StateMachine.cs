namespace EntityStateMachine
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set;}
        
        private bool isLockedState;
    
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
        public void LockOffStateMachine() => isLockedState = true;
    }
}
