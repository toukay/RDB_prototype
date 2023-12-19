namespace Cards.StateMachine
{
    public class CardStateMachine
    {
        public CardState CurrentState { get; private set; }

        public void Initialize(CardState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(CardState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}