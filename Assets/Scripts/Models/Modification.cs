using System;
using UnityEngine;

[Serializable]
public class Modification
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ModificationType _type;
    
    public string Name => _name;
    public Sprite Icon => _icon;
    public ModificationType Type => _type;
}