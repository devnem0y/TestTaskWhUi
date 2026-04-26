using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModificationPreviewView : PointerBehaviour
{
    [SerializeField] private TMP_Text _lblName;
    [SerializeField] private TMP_Text _lblType;
    [SerializeField] private Image _imgUnion;
    [SerializeField] private Image _imgIcon;

    public void Init(IModificationViewModel modificationVm, PlayerInput playerInput)
    {
        _lblName.text = modificationVm.Name;
        _lblType.text = modificationVm.Type.ToString();
        _imgIcon.sprite = modificationVm.Icon;
        _imgUnion.sprite = modificationVm.Union;
        
        SetupInputSystem(playerInput);
        
        transform.position = MousePosition;
    }

    protected override void OnMouseMoved(InputAction.CallbackContext context)
    {
        base.OnMouseMoved(context);
        UpdatePosition(MousePosition);
    }

    protected override void OnMouseLeftButtonUp(InputAction.CallbackContext context)
    {
        CleanupInputSystem();
        Destroy(gameObject);
    }

    private void UpdatePosition(Vector2 screenPosition)
    {
        transform.position = screenPosition;
    }
}