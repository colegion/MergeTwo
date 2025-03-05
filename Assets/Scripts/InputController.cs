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
            _selectedTile = temp;
            if (temp == _selectedTile)
            {
                _selectedTile.OnTap();
            }
            else
            {
                _selectedTile = temp;
                GameController.Instance.OnTapPerformed(_selectedTile);
            }
        }
        
        GameController.Instance.OnTapPerformed();
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

        Vector2 mouseScreenPos = GetPointerPosition(); 

        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos); 
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = _selectedTile.transform.position.y;
            _selectedTile.transform.position = targetPosition;
        }
    }
    
    private void OnHoldReleased(InputAction.CallbackContext context)
    {
        if (_selectedTile == null) return;
        Debug.Log("Hold released");

        Vector2 pointerPosition = GetPointerPosition();
        
        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPos = hit.point;
            
            int gridX = Mathf.FloorToInt(worldPos.x);
            int gridY = Mathf.FloorToInt(worldPos.z);
            
            BaseCell cell = GameController.Instance.GetCell(gridX, gridY);

            if (cell != null)
            {
                var tempTile = cell.GetTile(0);
                if (tempTile != null)
                {
                    GameController.Instance.OnSwipeReleased(_selectedTile, tempTile);
                }
                else
                {
                    Debug.LogWarning("grid x and y :" + gridX + ", grid y:" + gridY);
                    GameController.Instance.OnSwipeReleased(_selectedTile, new Vector2Int(gridX, gridY));
                }
            }
            else
            {
                Debug.LogWarning("Invalid swipe");
            }
        }
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
