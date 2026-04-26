using System;
using R3;

public class DragDropService : IDisposable
{
    private readonly Character _character;
    private readonly CompositeDisposable _disposables = new();
    
    private IAbilityViewModel _sourceAbility;
    
    public ReactiveProperty<IModificationViewModel> DraggedModification { get; } = new();
    public ReadOnlyReactiveProperty<bool> IsDragging => DraggedModification
        .Select(m => m != null).ToReadOnlyReactiveProperty();
    
    public void StartDraggingModification(IModificationViewModel modification, IAbilityViewModel sourceAbility = null)
    {
        DraggedModification.Value = modification;
        _sourceAbility = sourceAbility;
    }

    public void CancelDragging()
    {
        DraggedModification.Value = null;
        _sourceAbility = null;
    }

    public void CompleteDragDrop(IAbilityViewModel targetAbility)
    {
        if (DraggedModification.Value != null && targetAbility != null)
        {
            if (_sourceAbility != null && _sourceAbility != targetAbility) _sourceAbility.RemoveModification();
            
            targetAbility.ApplyModification(DraggedModification.Value);
        }

        CancelDragging();
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}