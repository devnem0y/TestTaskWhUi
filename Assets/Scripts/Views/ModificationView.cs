using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using R3;

public class ModificationView : ViewBase<IModificationViewModel>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [SerializeField] private TMP_Text _lblName;
    [SerializeField] private TMP_Text _lblType;
    [SerializeField] private Image _imgUnion;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private Image _imgFrameSelected;
    [SerializeField] private Image _imgBoard;
    [SerializeField] private Color _colorNormal;
    [SerializeField] private Color _colorCompatible;

    protected override void OnViewModelSet()
    {
        _lblName.text = ViewModel.Name;
        _lblType.text = ViewModel.Type.ToString();
        _imgIcon.sprite = ViewModel.Icon;
        _imgUnion.sprite = ViewModel.Union;
        
        ViewModel.IsApplied.Subscribe(Fade).AddTo(_viewDisposables);
        ViewModel.IsHovered.Subscribe(UpdateHoverVisual).AddTo(_viewDisposables);
        ViewModel.HasCompatibleHoveredAbility.Subscribe(UpdateCompatibilityVisual).AddTo(_viewDisposables);
    }

    protected override void OnMouseLeftButtonDown(InputAction.CallbackContext context)
    {
        if (!IsHovered || ViewModel.IsApplied.Value) return;
        
        IsClicked = true;
        ViewModel.OnDragStart();
        Fade(true);
    }

    protected override void OnMouseLeftButtonUp(InputAction.CallbackContext context)
    {
        if (IsClicked) IsClicked = false;
        
        if (!ViewModel.IsApplied.Value) Fade(false);
        
        if (ViewModel.DragDropService.IsDragging.CurrentValue &&
            ViewModel.DragDropService.DraggedModification.Value == ViewModel)
        {
            ViewModel.DragDropService.CancelDragging();
        }
    }
    
    protected override void OnEnter()
    {
        if (IsClicked) return;
        
        ViewModel.SetHoveredState(true);
    }

    protected override void OnExit()
    {
        ViewModel.SetHoveredState(false);
    }
    
    private void Fade(bool fade)
    {
        _canvasGroup.alpha = fade ? 0.5f : 1f;
    }
    
    private void UpdateCompatibilityVisual(bool hasCompatible)
    {
        _imgBoard.color = hasCompatible && !ViewModel.DragDropService.IsDragging.CurrentValue 
            ? _colorCompatible : _colorNormal;
    }
    
    private void UpdateHoverVisual(bool isHovered)
    {
        if (isHovered)
        {
            if (!ViewModel.IsApplied.Value) _imgFrameSelected.enabled = true;
        }
        else _imgFrameSelected.enabled = false;
    }
}