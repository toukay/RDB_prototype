using System.Collections;
using Cards.InputSystem;
using Cards.StateMachine.SubStates;
using Cards.StateMachine.SuperStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.StateMachine
{
    public class Card : MonoBehaviour
    {
        #region States
        public CardStateMachine StateMachine { get; private set; }
        public CardIdleState IdleState { get; private set; }
        public CardHoverState HoverState { get; private set; }
        public CardPressState PressState { get; private set; }
        public CardSelectedState SelectedState { get; private set; }
        public CardDragState DragState { get; private set; }
        public CardPlayState PlayState { get; private set; }
        
        #endregion
        
        #region Components

        public CardInputHandler inputHandler;
        
        [SerializeField] private float startDrawScale = 0.05f;
        [SerializeField] private float mouseHoverScale = 1.3f;
        [SerializeField] private float mouseHoverHeight = 1f;
        [SerializeField] private float mouseHoverSpeed = 15f;
        [SerializeField] private float mouseDragSpeed = .0f;
        [SerializeField] private float handMovementSpeed = 4f;
        [SerializeField] private float handRotationSpeed = 20f;
        [SerializeField] private float scaleSpeed = 8f;
        
        #endregion
        
        public bool IsHovering { get; private set; }
        public bool IsDragging => inputHandler != null && inputHandler.DragInput;
        
        public float StartDrawScale => startDrawScale;
        public float MouseHoverScale => mouseHoverScale;
        public float MouseHoverHeight => mouseHoverHeight;
        public float MouseHoverSpeed => mouseHoverSpeed;
        public float MouseDragSpeed => mouseDragSpeed;
        public float HandMovementSpeed => handMovementSpeed;
        public float HandRotationSpeed => handRotationSpeed;
        public float ScaleSpeed => scaleSpeed;
        
        public SpriteRenderer[] SpriteRenderers { get; private set; }
        
        #region Motion
        
        private Vector3? _targetPosition;
        private Quaternion? _targetRotation;
        private Vector3? _targetScale;
        private float _targetPositionSpeed;
        
        private const float TransitionThreshold = 0.05f;
        
        #endregion
        
        // Initializing States
        private void Awake()
        {
            StateMachine = new CardStateMachine();
            
            IdleState = new CardIdleState(StateMachine, this);
            HoverState = new CardHoverState(StateMachine, this);
            PressState = new CardPressState(StateMachine, this);
            SelectedState = new CardSelectedState(StateMachine, this);
            DragState = new CardDragState(StateMachine, this);
            PlayState = new CardPlayState(StateMachine, this);
            
            if (inputHandler == null)
            {
                // Try to get the component from anywhere in the scene
                inputHandler = FindObjectOfType<CardInputHandler>();

                // Optionally, log a warning if it's still not found
                if (inputHandler == null)
                {
                    Debug.LogWarning("CardInputHandler Not Found.");
                }
            }
            
            SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
        
        private void Start()
        {
            // inputHandler = GetComponent<CardInputHandler>();
            StateMachine.Initialize(IdleState);
        }
        
        private void Update()
        {
            StateMachine.CurrentState.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.FixedUpdate();
        }
        
        private void OnMouseEnter()
        {
            IsHovering = true;
        }

        private void OnMouseExit()
        {
            IsHovering = false;
        }
        
        public bool IsThisPressedCard()
        {
            return inputHandler.ClickedCardObject == this.gameObject;
        }
        
        #region Motion


        public bool isMotionDone()
        {
            return !_targetScale.HasValue && !_targetPosition.HasValue && !_targetRotation.HasValue;
        }
        
        public void RotateTo(Vector3 newEulerRotation)
        {
            _targetRotation = Quaternion.Euler(newEulerRotation);
        }

        public void MoveTo(Vector3 newPosition, float speed)
        {
            _targetPosition = newPosition;
            _targetPositionSpeed = speed;
        }


        public void ScaleTo(Vector3 newScale)
        {
            _targetScale = newScale;
        }
        
        public void UpdateHandScale()
        {
            if (!_targetScale.HasValue) return;
            
            float scaleAmount = scaleSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale.Value, scaleAmount);
            
            if (Vector3.Distance(transform.localScale, _targetScale.Value) < TransitionThreshold)
            {
                transform.localScale = _targetScale.Value;
                _targetScale = null;
            }
        }
        
        public void UpdateHandPosition()
        {
            if (!_targetPosition.HasValue) return;
            
            float moveAmount = _targetPositionSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _targetPosition.Value, moveAmount);
            
            if (Vector3.Distance(transform.position, _targetPosition.Value) <= TransitionThreshold)
            {
                transform.position = _targetPosition.Value;
                _targetPosition = null;
            }
        }

        public void UpdateHandRotation()
        {
            if (!_targetRotation.HasValue) return;

            float rotateAmount = handRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation.Value, rotateAmount);

            // Check if the rotation is complete
            if (Quaternion.Angle(transform.rotation, _targetRotation.Value) <= TransitionThreshold)
            {
                transform.rotation = _targetRotation.Value;
                _targetRotation = null;
            }
        }

        

        #endregion
    }
}