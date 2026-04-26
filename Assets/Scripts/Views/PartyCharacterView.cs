using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartyCharacterView : PointerBehaviour
{
    [SerializeField] private Image _imgIcon;
    
    public readonly Subject<Unit> OnClicked = new();

    private CharacterViewModel _characterVM;

    public void SetCharacterViewModel(CharacterViewModel characterViewModel, PlayerInput playerInput)
    {
        _characterVM = characterViewModel;
        _imgIcon.sprite = _characterVM.PartyIcon;
        
        SetupInputSystem(playerInput);
    }

    protected override void OnMouseLeftButtonDown(InputAction.CallbackContext context)
    {
        if (IsHovered) OnClicked.OnNext(Unit.Default);
    }

    private void OnDestroy()
    {
        CleanupInputSystem();
    }
}