using System;
using UnityEngine;

[Serializable]
public class Modification
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Sprite _union;
    [SerializeField] private Sprite _unionAbility;
    [SerializeField] private ModificationType _type;
    
    public string Name => _name;
    public Sprite Icon => _icon;
    public Sprite Union => _union;
    public Sprite UnionAbility => _unionAbility;
    public ModificationType Type => _type;
}