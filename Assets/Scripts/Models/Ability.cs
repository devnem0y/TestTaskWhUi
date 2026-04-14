using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<ModificationType> _modificationTypes;
}