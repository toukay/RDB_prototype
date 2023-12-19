using System.Collections;
using UnityEngine;

namespace Cards.StateMachine.SuperStates
{
    public class CardDrawState : CardState
    {

        private bool _isDrawDone;
        
        #region Motion
        
        private Vector3? _targetRotation;
        private Vector3? _targetPosition;
        private float _rotationSpeed;
        private float _movementSpeed;
        private bool _isRotating;
        private bool _isMoving;
        private float _moveProgress;
        private const float RotationThreshold = 0.05f;
        
        #endregion
        public CardDrawState(CardStateMachine stateMachine, Card card) : base(stateMachine, card) { }
        
        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            _isDrawDone = false;
            Debug.Log("Entered Draw State");
        }

        public override void Exit()
        {
            base.Exit();
        }
        
        public override void Update()
        {
            base.Update();
            
            UpdateRotation();
            UpdatePosition();
            
            if (!_isRotating && !_isMoving)
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
            _isDrawDone = true;
        }
        
        #region Motion

        public void RotateTo(Vector3 rotation, float speed)
        {
            _targetRotation = rotation;
            _rotationSpeed = speed;
            _isRotating = true;
        }

        public void MoveTo(Vector3 position, float speed, float delay = 0f)
        {
            card.StartCoroutine(MoveToAfterDelay(position, speed, delay));
        }

        private IEnumerator MoveToAfterDelay(Vector3 position, float speed, float delay)
        {
            yield return new WaitForSeconds(delay);
            _targetPosition = position;
            _movementSpeed = speed;
            _isMoving = true;
            _moveProgress = 0f;
        }

        private void UpdateRotation()
        {
            if (!_isRotating || !_targetRotation.HasValue) return;

            Quaternion currentRotation = card.transform.rotation;
            Quaternion targetQuaternion = Quaternion.Euler(_targetRotation.Value);
            card.transform.rotation = Quaternion.RotateTowards(currentRotation, targetQuaternion, _rotationSpeed * Time.deltaTime);

            // Check if the rotation is complete
            if (Quaternion.Angle(currentRotation, targetQuaternion) <= RotationThreshold)
            {
                _isRotating = false;
                _targetRotation = null;
                // Optional: Invoke an action or event here
            }
        }

        private void UpdatePosition()
        {
            if (!_isMoving || !_targetPosition.HasValue) return;

            // Increment the move progress based on time and speed
            _moveProgress += Time.deltaTime * _movementSpeed;

            // Use Lerp for smooth transition
            card.transform.position = Vector3.Lerp(card.transform.position, _targetPosition.Value, _moveProgress);

            // Check if the move is complete
            if (_moveProgress >= 1f)
            {
                _isMoving = false;
                _targetPosition = null;
            }
        }

        #endregion
    }
}