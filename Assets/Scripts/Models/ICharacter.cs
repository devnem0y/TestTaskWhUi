using System;
using ObservableCollections;
using R3;
using UnityEngine;

public interface ICharacter
{
    public ReadOnlyReactiveProperty<string> Name { get; }
    public ReadOnlyReactiveProperty<int> HP { get; }
    public ReadOnlyReactiveProperty<int> MaxHP { get; }
    public ReadOnlyReactiveProperty<int> Armor { get; }
    public ReadOnlyReactiveProperty<int> MaxArmor { get; }
    public ReadOnlyReactiveProperty<Sprite> PartyIcon { get; }
    public ReadOnlyReactiveProperty<Sprite> Avatar { get; }
    public ReadOnlyReactiveProperty<string> HealthDisplay { get; }
    public IObservableCollection<Ability> Abilities { get; }
    public IObservableCollection<Modification> Modifications { get; }

    public event Action<Ability> OnAbilityModified;
    
    public bool ApplyModificationToAbility(Ability ability, Modification modification);
    public void RemoveModificationFromAbility(Ability ability);
}