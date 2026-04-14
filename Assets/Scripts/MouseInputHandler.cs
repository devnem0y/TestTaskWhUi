using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    
    private InputAction _mousePositionAction;
    private InputAction _leftClickAction;

    private void Start()
    {
        _mousePositionAction = _playerInput.actions["Point"];
        _leftClickAction = _playerInput.actions["Click"];

        _leftClickAction.performed += OnLeftClick;
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        var mousePosition = _mousePositionAction.ReadValue<Vector2>();
        Debug.Log($"Left Click: <Mouse position> X={mousePosition.x}, Y={mousePosition.y}");
    }

    private void OnDestroy()
    {
        if (_leftClickAction != null) _leftClickAction.performed -= OnLeftClick;
    }
}
