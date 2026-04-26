using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<ModificationType> _compatibleModificationTypes;
    
    public string Name => _name;
    public Sprite Icon => _icon;
    public Modification AppliedModification { get; set; }

    public bool CanApplyModification(Modification modification)
    {
        if (modification == null) return false;
        
        if (!_compatibleModificationTypes.Contains(modification.Type)) return false;

        if (AppliedModification != null) return AppliedModification == modification;
        
        return true;
    }

    public void ApplyModification(Modification modification)
    {
        if (!CanApplyModification(modification)) return;
        
        AppliedModification = modification;
        modification.IsApplied = true;
    }

    public void RemoveModification()
    {
        if (AppliedModification == null) return;
        
        AppliedModification.IsApplied = false;
        AppliedModification = null;
    }
}
