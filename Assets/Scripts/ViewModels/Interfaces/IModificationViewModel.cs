using R3;
using UnityEngine;

public interface IModificationViewModel
{
    public string Name { get; }
    public Sprite Icon { get; }
    public Sprite Union { get; }
    public Sprite UnionAbility { get; }
    public ModificationType Type { get; }
    
    public Modification Model { get; }
    
    public ReactiveProperty<bool> IsApplied { get; }
    public ReadOnlyReactiveProperty<bool> IsHovered { get; }
    public ReadOnlyReactiveProperty<bool> HasCompatibleHoveredAbility { get; }
    public DragDropService DragDropService { get; }

    public void OnDragStart();
    public void SetHoveredState(bool isHovered);
    public void UpdateCompatibilityWithAbility(bool isCompatible);
}