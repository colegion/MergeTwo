using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private GameInputActions _inputActions;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private bool _isSwiping = false;
    private BaseTile _selectedTile;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _inputActions = new GameInputActions();
        _inputActions.Enable();
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        _inputActions.ActionMap.Tap.performed += OnTapPerformed;
        _inputActions.ActionMap.HoldToDrag.performed += OnHoldStarted;
        _inputActions.ActionMap.HoldToDrag.canceled += OnHoldReleased;
        _inputActions.ActionMap.Swipe.performed += OnSwiping;
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Tap detected");
        _startPosition = GetPointerPosition();

        Ray ray = mainCamera.ScreenPointToRay(_startPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var temp = hit.collider.GetComponent<BaseTile>();
            if (temp == _selectedTile)
            {
                _selectedTile.OnTap();
            }
            else
            {
                _selectedTile = temp;
            }
        }
    }

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Hold started");
        if (_selectedTile == null) return;

        _isSwiping = true;
    }

    private void OnSwiping(InputAction.CallbackContext context)
    {
        if (!_isSwiping || _selectedTile == null) return;

        Vector2 delta = context.ReadValue<Vector2>();
        Vector3 worldDelta = new Vector3(delta.x, 0, delta.y);

        _selectedTile.transform.position += worldDelta * Time.deltaTime;
    }

    private void OnHoldReleased(InputAction.CallbackContext context)
    {
        if (_selectedTile == null) return;
        Debug.Log("Hold released");

        _isSwiping = false;
        _selectedTile = null;
    }

    private Vector2 GetPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();
        return Mouse.current.position.ReadValue();
    }
}
