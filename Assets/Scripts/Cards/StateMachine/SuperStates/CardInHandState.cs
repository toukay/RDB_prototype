using UnityEngine;

namespace Cards.StateMachine.SuperStates
{
    public class CardInHandState : CardState
    {
        protected bool isCustomMotionDone;
        
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

            isCustomMotionDone = true;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (isCustomMotionDone)
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
            OriginalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            OriginalEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            OriginalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
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