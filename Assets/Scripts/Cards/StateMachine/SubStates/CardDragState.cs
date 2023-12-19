using Cards.StateMachine.SuperStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cards.StateMachine.SubStates
{
    public class CardDragState : CardInHandState
    {
        private bool _isDragDone;
        private Camera _mainCamera;
        private Vector3 _velocity = Vector3.zero;
        private float _mouseDragSpeed;
        
        private Vector3 _originalPosition;
        private Vector3 _originalEulerAngles;
        private Vector3 _originalScale;
        
        public CardDragState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            
            _isDragDone = false;
            Debug.Log("Entered Drag State");
            
            _mainCamera = card.inputHandler.MainCamera;
            _mouseDragSpeed = card.MouseDragSpeed;
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
            
            card.UpdateHandScale();
            
            DragUpdate();
            
            if (_isDragDone)
            {
                stateMachine.ChangeState(card.IdleState);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        private void DragUpdate()
        {
            if (!card.IsThisPressedCard())
            {
                _isDragDone = true;
                return;
            }
        
            var position = card.transform.position;
        
            float initialDistance = Vector3.Distance(position, _mainCamera.transform.position);
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 targetPoint = ray.GetPoint(initialDistance);
        
            card.transform.position =
                Vector3.SmoothDamp(position, targetPoint, ref _velocity, _mouseDragSpeed);
        }

        private void SetRotation()
        {
            
        }
    }
}