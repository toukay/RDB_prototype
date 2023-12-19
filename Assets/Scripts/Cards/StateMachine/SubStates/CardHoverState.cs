using Cards.StateMachine.SuperStates;
using UnityEngine;

namespace Cards.StateMachine.SubStates
{
    public class CardHoverState : CardInHandState
    {
        private bool _isHovering;
        private bool _pressInput;
        
        public CardHoverState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            // card.transform.localPosition += new Vector3(0, 1, 0);
            Debug.Log("Entered Hover State");

            SaveOriginalTransformValues();
            RenderFirst();
            SetScale();
            SetPosition();
            // SetRotation();
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void Update()
        {
            base.Update();
            
            _isHovering = card.IsHovering;
            _pressInput = card.inputHandler.PressInput;
            
            card.UpdateHandScale();
            card.UpdateHandRotation();
            card.UpdateHandPosition();
            
            if (!_isHovering)
            {
                // card.transform.localPosition += new Vector3(0, -1, 0);
                ResetRender();
                ResetTransformValues();
                stateMachine.ChangeState(card.IdleState);
            }
            else if (_pressInput && card.IsThisPressedCard())
            {
                stateMachine.ChangeState(card.PressState);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void SetScale()
        {
            card.ScaleTo(card.transform.localScale * card.MouseHoverScale);
        }
        
        private void SetPosition()
        {
            Camera camera = card.inputHandler.MainCamera;
            if (card.SpriteRenderers == null || card.SpriteRenderers.Length == 0) return;

            SpriteRenderer mainSpriteRenderer = card.SpriteRenderers[0];
            float halfCardHeight = mainSpriteRenderer.bounds.size.y / 2;
            Vector3 screenBottomEdge = camera.ScreenToWorldPoint(Vector3.zero);
            Vector3 screenTopEdge = camera.ScreenToWorldPoint(new Vector3(0, Screen.height));
    
            bool isCloserToBottomEdge = CloserEdge(card.transform, camera, Screen.width, Screen.height) == 1;
            Vector3 edgePosition = isCloserToBottomEdge ? screenBottomEdge : screenTopEdge;
    
            Vector3 newPosition = new Vector3(
                card.transform.position.x, 
                edgePosition.y + (halfCardHeight + card.MouseHoverHeight) * (isCloserToBottomEdge ? 1 : -1), 
                card.transform.position.z
            );

            card.MoveTo(newPosition, card.MouseHoverSpeed);
        }

        private void SetRotation()
        {
            
        }
        
        public int CloserEdge(Transform transform, Camera camera, int width, int height)
        {
            //edge points according to the screen/camera
            Vector3 worldPointTop = camera.ScreenToWorldPoint(new Vector3(width / 2, height));
            Vector3 worldPointBot = camera.ScreenToWorldPoint(new Vector3(width / 2, 0));

            //distance from the pivot to the screen edge
            float deltaTop = Vector2.Distance(worldPointTop, transform.position);
            float deltaBottom = Vector2.Distance(worldPointBot, transform.position);

            return deltaBottom <= deltaTop ? 1 : -1;
        }
    }
}