using UnityEngine;

namespace Cards.StateMachine.SuperStates
{
    public class CardInHandState : CardState
    {
        protected bool isTransitionDone;
        
        protected Vector3 OriginalPosition;
        protected Vector3 OriginalEulerAngles;
        protected Vector3 OriginalScale;
        
        public CardInHandState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            isTransitionDone = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (isTransitionDone)
            {
                card.UpdateHandScale();
                card.UpdateHandRotation();
                card.UpdateHandPosition();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        protected void SaveOriginalTransformValues()
        {
            Debug.Log("OK SAVING THIS POSS");
            Transform transform = card.transform;
            OriginalPosition = transform.position;
            OriginalEulerAngles = transform.eulerAngles;
            OriginalScale = transform.localScale;
        }
        
        protected void ResetTransformValues()
        {
            Debug.Log("OK RETURNING THERE!!");
            card.ScaleTo(OriginalScale);
            card.MoveTo(OriginalPosition, card.MouseHoverSpeed);
        }
        
        protected void RenderFirst()
        {
            foreach (SpriteRenderer sr in card.SpriteRenderers)
                sr.sortingOrder += 1;
        }

        protected void ResetRender()
        {
            foreach (SpriteRenderer sr in card.SpriteRenderers)
                sr.sortingOrder -= 0;
        }
    }
}