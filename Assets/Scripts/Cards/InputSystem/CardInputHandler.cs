using Cards.StateMachine;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Cards.InputSystem
{
    public class CardInputHandler : MonoBehaviour
    {
        #region Properties
        public bool PressInput { get; private set; }
        public bool SingleClickInput { get; private set; }
        public bool DragInput { get; private set; }
        
        public GameObject ClickedCardObject { get; private set; }
        
        public Camera MainCamera  { get; private set; }
        
        #endregion
        
        private void Awake()
        {
            Debug.Log("Drag input: " + DragInput);
            MainCamera = Camera.main;
        }

        public void OnPressInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PressInput = true;
                LoadPressedCard();
            }

            if (context.performed)
            {
                // Only Entered after `Hold time` and `Press point` of `Hold` interaction specified in the input action editor
                DragInput = true;
            }

            if (context.canceled)
            {
                ClickedCardObject = null;
                PressInput = false;
                if (!DragInput)
                {
                    SingleClickInput = true;
                }
                else
                {
                    DragInput = false;
                }
            }
        }

        private void LoadPressedCard()
        {
            
            Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
            if (hit2D.collider != null && (hit2D.collider.gameObject.CompareTag("Card")
                                           || hit2D.collider.gameObject.GetComponent<Card>() != null))
            {
                ClickedCardObject = hit2D.collider.gameObject;
            }
        }
        
        public void UseSingleClickInput() => SingleClickInput = false;
    }
}