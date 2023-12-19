using Cards.StateMachine.SuperStates;
using UnityEngine;

namespace Cards.StateMachine.SubStates
{
    public class CardSelectedState : CardInHandState
    {
        private bool _isSelectedDone;
        public CardSelectedState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            _isSelectedDone = false;
            
            Debug.Log("Entered Selected State");
            
            card.inputHandler.UseSingleClickInput();
        }

        public override void Exit()
        {
            base.Exit();
            
            ResetRender();
            ResetTransformValues();
        }
        
        public override void Update()
        {
            base.Update();

            DoSomething();
            
            if (_isSelectedDone)
            {
                stateMachine.ChangeState(card.IdleState);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        private void DoSomething()
        {
            _isSelectedDone = true;
        }
    }
}