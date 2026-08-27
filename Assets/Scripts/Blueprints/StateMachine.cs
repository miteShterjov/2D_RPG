using EntityStateMachine;

namespace Blueprints
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set;}
    
        public void Initialize(EntityState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }
    
        public void ChangeState(EntityState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateActiveState() => CurrentState.Update();
    }
}
