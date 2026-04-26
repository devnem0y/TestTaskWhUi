using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using R3;

public class AbilityView : ViewBase<IAbilityViewModel>
{
    [SerializeField] private TMP_Text _lblName;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private Image _imgModificationUnion;
    [SerializeField] private Image _imgModificationIcon;
    [SerializeField] private Image _imgArrow;

    protected override void OnViewModelSet()
    {
        _lblName.text = ViewModel.Name;
        _imgIcon.sprite = ViewModel.Icon;
        
        ViewModel.IsHovered.Subscribe(_ => UpdateVisualState()).AddTo(_viewDisposables);
        ViewModel.HasCompatibleHoveredModification.Subscribe(_ => UpdateVisualState()).AddTo(_viewDisposables);
        ViewModel.IsDropTarget.Subscribe(_ => { UpdateVisualState(); }).AddTo(_viewDisposables);
        ViewModel.AppliedModification
            .Subscribe(mod => UpdateAppliedState(mod != null)).AddTo(_viewDisposables);
        
    }

    protected override void OnMouseLeftButtonDown(InputAction.CallbackContext context)
    {
        if (!IsHovered) return;

        IsClicked = true;

        ViewModel.OnDragStart();
    }

    protected override void OnMouseLeftButtonUp(InputAction.CallbackContext context)
    {
        if (IsClicked) IsClicked = false;
        
        if (!ViewModel.DragDropService.IsDragging.CurrentValue) return;
        
        if (IsHovered)
        {
            ViewModel.OnDrop();
            return;
        }
        
        if (ViewModel.DragDropService.DraggedModification.Value != ViewModel.AppliedModification.CurrentValue) return;
        
        ViewModel.RemoveModification();
    }

    protected override void OnEnter()
    {
        ViewModel.SetHoveredState(true);
    }

    protected override void OnExit()
    {
        ViewModel.SetHoveredState(false);
    }
    
    private void UpdateVisualState()
    {
        _imgIcon.color = Color.white;
        _imgArrow.gameObject.SetActive(false);

        if (ViewModel.IsDropTarget.CurrentValue) SetCompatibleState();
        else
        {
            if (ViewModel.IsHovered.CurrentValue) _imgIcon.color = Color.gray;

            if (!ViewModel.HasCompatibleHoveredModification.CurrentValue) return;

            SetCompatibleState();
        }
    }

    private void UpdateAppliedState(bool hasModification)
    {
        if (hasModification)
        {
            _imgModificationIcon.sprite = ViewModel.AppliedModification.CurrentValue.Icon;
            _imgModificationUnion.sprite = ViewModel.AppliedModification.CurrentValue.UnionAbility;
        }

        _imgModificationUnion.gameObject.SetActive(hasModification);
    }
    
    private void SetCompatibleState()
    {
        _imgIcon.color = Color.green;
        _imgArrow.color = Color.green;
        _imgArrow.gameObject.SetActive(true);
    }
}