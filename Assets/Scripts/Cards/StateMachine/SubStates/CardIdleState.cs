using Cards.StateMachine.SuperStates;
using UnityEngine;

namespace Cards.StateMachine.SubStates
{
    public class CardIdleState : CardInHandState
    {
        private bool _isHovering;
        
        public CardIdleState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Entered Idle State");

            isTransitionDone = true;
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void Update()
        {
            base.Update();
            
            _isHovering = card.IsHovering;
            
            if (_isHovering)
            {
                stateMachine.ChangeState(card.HoverState);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}