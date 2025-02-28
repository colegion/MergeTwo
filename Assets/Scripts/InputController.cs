using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private GameInputActions _inputActions;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private bool _isSwiping;
    private BaseTile _selectedTile;

    public void Initialize()
    {
        _inputActions = new GameInputActions();
        _inputActions.Enable();
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        _inputActions.ActionMap.Tap.performed += OnTapPerformed;
        _inputActions.ActionMap.Swipe.started += OnSwipeStarted;
        _inputActions.ActionMap.Swipe.performed += OnSwiping;
        _inputActions.ActionMap.Swipe.canceled += OnSwipeReleased;
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        _startPosition = Mouse.current.position.ReadValue();

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            _startPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        Ray ray = mainCamera.ScreenPointToRay(_startPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _selectedTile = hit.collider.GetComponent<BaseTile>();
            GameController.Instance.OnTapPerformed(_selectedTile);
        }
        else
        {
            GameController.Instance.OnTapPerformed();
        }
    }

    private void OnSwipeStarted(InputAction.CallbackContext context)
    {
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

    private void OnSwipeReleased(InputAction.CallbackContext context)
    {
        if (_selectedTile == null) return;

        _endPosition = Mouse.current.position.ReadValue();
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            _endPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        _isSwiping = false;
        
        Ray ray = mainCamera.ScreenPointToRay(_endPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            BaseTile targetTile = hit.collider.GetComponent<BaseTile>();

            if (targetTile != null)
            {
            }
            else
            {
            }
        }

        _selectedTile = null;
    }
}
