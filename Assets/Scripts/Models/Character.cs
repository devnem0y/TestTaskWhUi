using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Character
{
    public string Name { get; }
    public int HP { get; }
    public int MaxHP { get; }
    public int Armor { get; }
    public int MaxArmor { get; }
    public Sprite PartyIcon { get; }
    public Sprite Portrait { get; }
    public List<Ability> Abilities { get; }
    public List<Modification> Modifications { get; }

    public Character(CharacterConfig characterConfig, AbilitiesConfig abilitiesConfig, ModificationsConfig modificationsConfig)
    {
        Name = characterConfig.Name;
        MaxHP = characterConfig.Hp;
        MaxArmor = characterConfig.Armor;
        Portrait = characterConfig.Portrait;
        PartyIcon = characterConfig.PartyIcon;
        
        Abilities = new List<Ability>(abilitiesConfig.GetAbilities(characterConfig.AbilitySize));
        Modifications = new List<Modification>(modificationsConfig.GetModifications());
        
        //TODO: Случайные значени для иметации данных
        HP = Random.Range(1, MaxHP);
        Armor = Random.Range(1, MaxArmor);
    }
}