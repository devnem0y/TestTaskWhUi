using System;
using ObservableCollections;
using UnityEngine;

public interface ICharacter
{
    public string Name { get; }
    public int HP { get; }
    public int MaxHP { get; }
    public int Armor { get; }
    public int MaxArmor { get; }
    public Sprite PartyIcon { get; }
    public Sprite Portrait { get; }
    
    public IObservableCollection<Ability> Abilities { get; }
    public IObservableCollection<Modification> Modifications { get; }

    public event Action<Ability> OnAbilityModified;
    
    public bool ApplyModificationToAbility(Ability ability, Modification modification);
    public void RemoveModificationFromAbility(Ability ability);
}