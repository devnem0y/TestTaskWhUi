using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class PointerBehaviour : MonoBehaviour
{
    [SerializeField] private Graphic _raycastTarget;
    
    private InputAction _mousePositionAction;
    private InputAction _mouseClickAction;

    protected Vector2 MousePosition => _mousePositionAction?.ReadValue<Vector2>() ?? Vector2.zero;
    protected bool IsHovered { get; private set; }
    protected bool IsClicked { get; set; }
    
    private bool _isEntered;

    protected void SetupInputSystem(PlayerInput playerInput)
    {
        _mousePositionAction = playerInput.actions["Point"];
        _mousePositionAction.performed += OnMouseMoved;

        _mouseClickAction = playerInput.actions["Click"];
        _mouseClickAction.performed += OnMouseLeftButtonDown;
        _mouseClickAction.canceled += OnMouseLeftButtonUp;
    }

    protected void CleanupInputSystem()
    {
        if (_mousePositionAction != null)
        {
            _mousePositionAction.performed -= OnMouseMoved;
        }

        if (_mouseClickAction != null)
        {
            _mouseClickAction.performed -= OnMouseLeftButtonDown;
            _mouseClickAction.canceled -= OnMouseLeftButtonUp;
        }
    }

    protected virtual void OnMouseMoved(InputAction.CallbackContext context)
    {
        var isCurrentlyHovered = IsPointerOverThis();
        if (isCurrentlyHovered == IsHovered) return;
        
        IsHovered = isCurrentlyHovered;

        if (IsHovered != _isEntered)
        {
            if (IsHovered) OnEnter();
            else OnExit();
            _isEntered = IsHovered;
        }
    }
    
    protected virtual void OnMouseLeftButtonDown(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnMouseLeftButtonUp(InputAction.CallbackContext context)
    {
        
    }
    
    protected virtual void OnEnter()
    {
        
    }

    protected virtual void OnExit()
    {
        
    }
    
    private bool IsPointerOverThis()
    {
        var eventData = new PointerEventData(EventSystem.current) { position = MousePosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Any(result => result.gameObject == _raycastTarget.gameObject);
    }
}