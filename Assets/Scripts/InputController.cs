using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private GameInputActions _inputActions;

    public void Initialize()
    {
        _inputActions = new GameInputActions();
        _inputActions.Enable();
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        _inputActions.ActionMap.Tap.performed += OnTapPerformed;
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("tap performed");
        Vector2 screenPosition = Mouse.current.position.ReadValue();
            
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
            
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var tile = hit.collider.GetComponent<BaseTile>();
            GameController.Instance.OnTapPerformed(tile);
        }
        else
        {
            GameController.Instance.OnTapPerformed();
        }
    }
}
