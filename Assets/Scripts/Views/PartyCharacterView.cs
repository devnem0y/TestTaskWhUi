using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartyCharacterView : MonoBehaviour
{
    [SerializeField] private Image _iconImage; 
    
    private int _id;
    private InputAction _clickAction;
    
    public event Action<int> onSelected;

    public void Init(int id, Sprite icon, PlayerInput playerInput)
    {
        _id = id;
        _iconImage.sprite = icon;
        
        _clickAction = playerInput.actions["Click"];
        _clickAction.performed += OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (IsPointerOverThis())
        {
            onSelected?.Invoke(_id);
        }
    }

    private bool IsPointerOverThis()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Any(result => result.gameObject == gameObject);
    }

    private void OnDestroy()
    {
        _clickAction.performed -= OnClickPerformed;
    }
}