using UnityEngine;

namespace Cards.StateMachine
{
    public class CardState
    {
        protected CardStateMachine stateMachine;
        protected Card card;
        
        protected bool isExitingState;
        
        protected float startTime;
        
        public CardState(CardStateMachine stateMachine, Card card)
        {
            this.stateMachine = stateMachine;
            this.card = card;
        }

        // Enter this state. Called when entering the state.
        public virtual void Enter()
        {
            DoChecks();
            startTime = Time.time;
            isExitingState = false;
        }

        // Exit this state. Called when exiting the state.
        public virtual void Exit()
        {
            isExitingState = true;
        }

        // Update method called every frame.
        public virtual void Update() { }

        // FixedUpdate method called for physics updates.
        public virtual void FixedUpdate()
        {
            DoChecks();
        }

        // Perform various checks in this state.
        public virtual void DoChecks() { }
    }
}