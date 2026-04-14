using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    private string _name;
    private int _hp;
    private int _armor;
    private Sprite _partyIcon;
    private Sprite _avatar;
    
    private int _maxHp;
    private int _maxArmor;
    private List<Ability> _abilities;
    private List<Modification> _modifications;
    
    public Character(CharacterConfig characterConfig, AbilitiesConfig abilitiesConfig, ModificationsConfig modificationsConfig)
    {
        _name = characterConfig.Name;
        _hp = characterConfig.Hp;
        _armor = characterConfig.Armor;
        
        _maxHp = _hp;
        _maxArmor = _armor;
        
        _avatar = characterConfig.Avatar;
        _partyIcon = characterConfig.PartyIcon;

        _abilities = new List<Ability>(abilitiesConfig.GetAbilities(characterConfig.AbilitySize));
        _modifications = new List<Modification>(modificationsConfig.GetModifications());
    }
}