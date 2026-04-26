using System;
using R3;
using UnityEngine;

public class ModificationViewModel : IModificationViewModel, IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    
    public string Name => Model.Name;
    public Sprite Icon => Model.Icon;
    public Sprite Union => Model.Union;
    public Sprite UnionAbility => Model.UnionAbility;
    public ModificationType Type => Model.Type;
    
    public Modification Model { get; }

    public ReactiveProperty<bool> IsApplied { get; } = new();
    public ReadOnlyReactiveProperty<bool> IsHovered { get; }
    public ReadOnlyReactiveProperty<bool> HasCompatibleHoveredAbility { get; }
    public DragDropService DragDropService { get; }

    private readonly ReactiveProperty<bool> _isHovered = new(false);
    private readonly ReactiveProperty<bool> _hasCompatibleHoveredAbility = new(false);

    public ModificationViewModel(Modification model, DragDropService dragDropService)
    {
        Model = model;
        DragDropService = dragDropService;
        
        IsHovered = _isHovered.AddTo(_disposables);
        HasCompatibleHoveredAbility = _hasCompatibleHoveredAbility.AddTo(_disposables);
        
        IsApplied.Subscribe(applied => { Model.IsApplied = applied; }).AddTo(_disposables);
    }

    public void SetHoveredState(bool isHovered)
    {
        _isHovered.Value = isHovered;
    }
    
    public void UpdateCompatibilityWithAbility(bool isCompatible)
    {
        _hasCompatibleHoveredAbility.Value = isCompatible;
    }
    
    public void OnDragStart()
    {
        DragDropService.StartDraggingModification(this);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}