using R3;
using UnityEngine;

public interface IAbilityViewModel
{
    public string Name { get; }
    public Sprite Icon { get; }
    
    public Ability Model { get; }
    
    public ReactiveProperty<IModificationViewModel> AppliedModification { get; }
    public ReadOnlyReactiveProperty<bool> IsHovered { get; }
    public ReactiveProperty<bool> HasCompatibleHoveredModification { get; }
    public DragDropService DragDropService { get; }
    public ReadOnlyReactiveProperty<bool> IsDropTarget { get; }
    
    public void SetHoveredState(bool isHovered);
    public void ApplyModification(IModificationViewModel modificationVM);
    public void RemoveModification();
    public void OnDragStart();
    public void OnDrop();
}