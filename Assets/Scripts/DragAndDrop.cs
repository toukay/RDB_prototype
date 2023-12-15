using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private InputAction mouseClick;
    [SerializeField] private float mouseDragPhysicsSpeed = 10;
    [SerializeField] private float mouseDragSpeed = .1f;
    
    private Camera _mainCamera;
    private readonly WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 _velocity = Vector3.zero;
    

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit))
        // {
        //     if (hit.collider != null && (hit.collider.gameObject.CompareTag("Draggable")  || hit.collider.gameObject.GetComponent<IDraggable>() != null))
        //     {
        //         StartCoroutine(DragUpdate(hit.collider.gameObject));
        //     }
        // }

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider != null && (hit2D.collider.gameObject.CompareTag("Draggable") || hit2D.collider.gameObject.GetComponent<IDraggable>() != null))
        {
            StartCoroutine(DragUpdate(hit2D.collider.gameObject));
        }
    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, _mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        clickedObject.TryGetComponent<IDraggable>(out var draggable);
        draggable?.OnStartDrag();
        while (mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;
                yield return _waitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref _velocity, mouseDragSpeed);
                yield return null;
            }
        }
        draggable?.OnEndDrag();
    }
}
