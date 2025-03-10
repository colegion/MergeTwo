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

    private const float TapThreshold = 0.05f;

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
            targetPosition.y = .35f;
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

            Vector2Int gridCoords = GetGridCoordinates(worldPos);

            TryProcessRelease(gridCoords);
        }
        else
        {
            // If raycast fails, fallback to current position
            GameController.Instance.OnSwipeReleased(_selectedTile, new Vector2Int(_selectedTile.X, _selectedTile.Y));
        }
    }

    _selectedTile = null;
    _canSwipe = false;
}

/// <summary>
/// Converts world position to grid coordinates, rounding correctly.
/// </summary>
private Vector2Int GetGridCoordinates(Vector3 worldPos)
{
    float gridWidth = GameController.Instance.GridWidth;
    float gridHeight = GameController.Instance.GridHeight;

    Vector3 gridOrigin = new Vector3(
        -gridWidth / 2f,
        0,
        -gridHeight / 2f
    );

    int gridX = GetGridCoordinate(worldPos.x, gridOrigin.x);
    int gridY = GetGridCoordinate(worldPos.z, gridOrigin.z);

    return new Vector2Int(gridX, gridY);
}

private int GetGridCoordinate(float worldCoord, float gridOrigin)
{
    float localCoord = worldCoord - gridOrigin;
    return Mathf.FloorToInt(localCoord + 0.5f);
}

private void TryProcessRelease(Vector2Int gridCoords)
{
    
    BaseCell cell = GameController.Instance.GetCell(gridCoords.x, gridCoords.y);
    if (cell == null)
    {
        Debug.LogWarning("Invalid swipe. Finding nearest valid position.");

        Vector2Int clampedCoords = GetClampedGridCoordinates(gridCoords);
        cell = GameController.Instance.GetCell(clampedCoords.x, clampedCoords.y);

        if (cell != null)
        {
            gridCoords = clampedCoords;
        }
        else
        {
            Debug.LogWarning("No valid cell found even after clamping. Resetting to original tile position.");
            gridCoords = new Vector2Int(_selectedTile.X, _selectedTile.Y);
            cell = GameController.Instance.GetCell(gridCoords.x, gridCoords.y);
        }
    }
    
    if (cell != null)
    {
        var tempTile = cell.GetTile(0);

        if (tempTile != null)
        {
            GameController.Instance.OnSwipeReleased(_selectedTile, tempTile);
        }
        else
        {
            GameController.Instance.OnSwipeReleased(_selectedTile, gridCoords);
        }
    }
    else
    {
        Debug.LogError("Critical: Could not resolve a valid release position.");
        GameController.Instance.OnSwipeReleased(_selectedTile, new Vector2Int(_selectedTile.X, _selectedTile.Y));
    }
}


private Vector2Int GetClampedGridCoordinates(Vector2Int gridCoords)
{
    int gridWidth = GameController.Instance.GridWidth;
    int gridHeight = GameController.Instance.GridHeight;

    int clampedX = Mathf.Clamp(gridCoords.x, 0, gridWidth - 1);
    int clampedY = Mathf.Clamp(gridCoords.y, 0, gridHeight - 1);

    Debug.Log($"Clamped Grid Position: ({clampedX}, {clampedY})");

    return new Vector2Int(clampedX, clampedY);
}


    private Vector2 GetPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();
        return Mouse.current.position.ReadValue();
    }
}