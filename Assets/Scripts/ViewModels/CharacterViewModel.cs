using System;
using System.Collections.Specialized;
using System.Linq;
using ObservableCollections;
using R3;
using UnityEngine;

public class CharacterViewModel : IDisposable
{
    public string Name { get; }
    public int HP { get; }
    public int MaxHP { get; }
    public int Armor { get; }
    public int MaxArmor { get; }
    public Sprite PartyIcon { get; }
    public Sprite Portrait { get; }
    
    private Character Model { get; } //TODO: Пока временно прайватный

    public CharacterViewModel(Character model)
    {
        Model = model;
        
        Name = Model.Name;
        HP = Model.HP;
        MaxHP = Model.MaxHP;
        Armor = Model.Armor;
        MaxArmor = Model.MaxArmor;
        PartyIcon = Model.PartyIcon;
        Portrait = Model.Portrait;
    }

    public void Dispose()
    {
        
    }
}