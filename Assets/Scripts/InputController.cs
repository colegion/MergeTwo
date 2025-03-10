using System;
using GridSystem;
using Tile;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private GameInputActions _inputActions;

    private Vector2 _startPosition;
    private float _holdStartTime;
    private bool _canSwipe = false;
    private BaseTile _selectedTile;

    private const float TapThreshold = 0.2f;

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
        _inputActions.ActionMap.HoldToDrag.performed += OnHoldStarted;
        _inputActions.ActionMap.Swipe.performed += OnSwiping;
        _inputActions.ActionMap.HoldToDrag.canceled += OnHoldReleased;
    }

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        _startPosition = GetPointerPosition();
        _holdStartTime = Time.time;

        Ray ray = mainCamera.ScreenPointToRay(_startPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var temp = hit.collider.GetComponent<BaseTile>();
            if (temp != null)
            {
                if (temp == _selectedTile)
                {
                    _selectedTile.OnTap();
                }
                else
                {
                    _canSwipe = true;
                    _selectedTile = temp;
                    GameController.Instance.OnTapPerformed(_selectedTile);
                }
            }
        }
    }

    private void OnSwiping(InputAction.CallbackContext context)
    {
        if (!_canSwipe || _selectedTile == null) return;

        Vector2 pointerPosition = GetPointerPosition();

        float swipeThreshold = 5f;
        Vector2 swipeDelta = pointerPosition - _startPosition;

        if (swipeDelta.magnitude < swipeThreshold) return;

        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
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

    float holdDuration = Time.time - _holdStartTime;

    if (holdDuration < TapThreshold)
    {
        _selectedTile.OnTap();
    }
    else
    {
        Vector2 pointerPosition = GetPointerPosition();
        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPos = hit.point;

            int gridX = Mathf.FloorToInt(worldPos.x + (GameController.Instance.GridWidth / 2f));
            int gridY = Mathf.FloorToInt(worldPos.z + (GameController.Instance.GridHeight / 2f));

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
                    GameController.Instance.OnSwipeReleased(_selectedTile, new Vector2Int(gridX, gridY));
                }
            }
            else
            {
                Debug.LogWarning($"Invalid swipe. Finding nearest valid position.");
                
                float clampedX = Mathf.Clamp(worldPos.x, -GameController.Instance.GridWidth / 2f, GameController.Instance.GridWidth / 2f - 1);
                float clampedZ = Mathf.Clamp(worldPos.z, -GameController.Instance.GridHeight / 2f, GameController.Instance.GridHeight / 2f - 1);
                
                int nearestGridX = Mathf.FloorToInt(clampedX + (GameController.Instance.GridWidth / 2f));
                int nearestGridY = Mathf.FloorToInt(clampedZ + (GameController.Instance.GridHeight / 2f));

                Debug.Log($"Nearest Grid Position: ({nearestGridX}, {nearestGridY})");

                GameController.Instance.OnSwipeReleased(_selectedTile, new Vector2Int(nearestGridX, nearestGridY));
            }
        }
    }

    _selectedTile = null;
    _canSwipe = false;
}


    private Vector2 GetPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();
        return Mouse.current.position.ReadValue();
    }
}
