using System;
using UnityEngine;

[Serializable]
public class CharacterConfig 
{
    [SerializeField] private string _name;
    public string Name => _name;
    
    [SerializeField] private int _hp;
    public int Hp => _hp;
    
    [SerializeField] private int _armor;
    public int Armor => _armor;
    
    [SerializeField] private AbilitySize _abilitySize;
    public AbilitySize AbilitySize => _abilitySize;
    
    [SerializeField] private Sprite _partyIcon;
    public Sprite PartyIcon => _partyIcon;
    
    [SerializeField] private Sprite _portrait;
    public Sprite Portrait => _portrait;
}