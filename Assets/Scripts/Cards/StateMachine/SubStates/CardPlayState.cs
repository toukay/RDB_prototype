using Cards.StateMachine.SuperStates;

namespace Cards.StateMachine.SubStates
{
    public class CardPlayState : CardInHandState
    {
        public CardPlayState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}