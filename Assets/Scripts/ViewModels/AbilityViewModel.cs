using System;
using R3;
using UnityEngine;

public class AbilityViewModel : IAbilityViewModel, IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    
    public string Name => Model.Name;
    public Sprite Icon => Model.Icon;
    
    public Ability Model { get; }

    public ReactiveProperty<IModificationViewModel> AppliedModification { get; } = new();
    public ReadOnlyReactiveProperty<bool> IsHovered { get; }
    public ReactiveProperty<bool> HasCompatibleHoveredModification { get; }
    public DragDropService DragDropService { get; }
    public ReadOnlyReactiveProperty<bool> IsDropTarget { get; }
    
    private readonly ReactiveProperty<bool> _isHovered = new(false);
    private readonly ReactiveProperty<bool> _hasCompatibleHoveredModification = new(false);
    private readonly ReactiveProperty<bool> _isDropTarget = new(false);

    public AbilityViewModel(Ability model, DragDropService dragDropService)
    {
        Model = model;
        DragDropService = dragDropService;

        IsHovered = _isHovered.AddTo(_disposables);
        IsDropTarget = _isDropTarget.AddTo(_disposables);
        HasCompatibleHoveredModification = _hasCompatibleHoveredModification.AddTo(_disposables);
        AppliedModification.Subscribe(mod => Model.AppliedModification = mod?.Model).AddTo(_disposables);
        DragDropService.IsDragging.Subscribe(UpdateDropTargetState).AddTo(_disposables);
        DragDropService.DraggedModification
            .Subscribe(_ => UpdateDropTargetState(DragDropService.IsDragging.CurrentValue)).AddTo(_disposables);
    }

    public void SetHoveredState(bool isHovered)
    {
        _isHovered.Value = isHovered;
    }

    public void ApplyModification(IModificationViewModel modificationVM)
    {
        Model.ApplyModification(modificationVM.Model);
        AppliedModification.Value = modificationVM;
        modificationVM.IsApplied.Value = true;
    }

    public void RemoveModification()
    {
        Model.RemoveModification();
        
        if (AppliedModification.Value == null) return;
        
        AppliedModification.Value.IsApplied.Value = false;
        AppliedModification.Value = null;
    }
    
    public void OnDragStart()
    {
        if (AppliedModification.CurrentValue == null) return;
        
        DragDropService.StartDraggingModification(AppliedModification.CurrentValue, this);
    }
    
    public void OnDrop()
    {
        if (_isDropTarget.Value) DragDropService.CompleteDragDrop(this);
    }
    
    private void UpdateDropTargetState(bool isDragging)
    {
        if (isDragging && Model.CanApplyModification(DragDropService.DraggedModification.Value.Model)) 
            _isDropTarget.Value = true;
        else _isDropTarget.Value = false;
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
