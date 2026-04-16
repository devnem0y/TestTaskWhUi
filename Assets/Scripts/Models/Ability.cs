using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

[Serializable]
public class Ability
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<ModificationType> _compatibleModificationTypes;

    public string Name => _name;
    public Sprite Icon => _icon;
    public List<ModificationType> CompatibleModificationTypes => _compatibleModificationTypes;
    
    public ReactiveProperty<Modification> AppliedModificationReactive { get; } = new();

    public Modification AppliedModification
    {
        get => AppliedModificationReactive.Value;
        set => AppliedModificationReactive.Value = value;
    }
    
    public ModificationType? GetAppliedModificationType() => AppliedModification?.Type;
}
