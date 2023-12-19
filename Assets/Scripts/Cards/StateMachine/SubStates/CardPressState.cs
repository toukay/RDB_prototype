using Cards.StateMachine.SuperStates;
using UnityEngine;

namespace Cards.StateMachine.SubStates
{
    public class CardPressState : CardInHandState
    {
        
        private bool _dragInput;
        private bool _singleClickInput;
            
        public CardPressState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Entered Press State");
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void Update()
        {
            base.Update();
            
            _singleClickInput = card.inputHandler.SingleClickInput;
            _dragInput = card.inputHandler.DragInput;
            
            if (_dragInput) 
            {
                stateMachine.ChangeState(card.DragState);
            }
            else if (_singleClickInput)
            {
                stateMachine.ChangeState(card.SelectedState);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}