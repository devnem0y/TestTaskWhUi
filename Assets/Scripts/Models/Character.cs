using System;
using ObservableCollections;
using R3;
using UnityEngine;

[Serializable]
public class Character : ICharacter
{
    private readonly string _name;
    private readonly int _hp;
    private readonly int _armor;
    private readonly int _maxHp;
    private readonly int _maxArmor;
    private readonly Sprite _partyIcon;
    private readonly Sprite _avatar;
    
    private readonly ObservableList<Ability> _abilities;
    private readonly ObservableList<Modification> _modifications;
    
    public ReadOnlyReactiveProperty<string> Name { get; }
    public ReadOnlyReactiveProperty<int> HP { get; }
    public ReadOnlyReactiveProperty<int> MaxHP { get; }
    public ReadOnlyReactiveProperty<int> Armor { get; }
    public ReadOnlyReactiveProperty<int> MaxArmor { get; }
    public ReadOnlyReactiveProperty<Sprite> PartyIcon { get; }
    public ReadOnlyReactiveProperty<Sprite> Avatar { get; }
    public ReadOnlyReactiveProperty<string> HealthDisplay { get; }
    public IObservableCollection<Ability> Abilities => _abilities;
    public IObservableCollection<Modification> Modifications => _modifications;
    
    public event Action<Ability> OnAbilityModified;
    
    public Character(CharacterConfig characterConfig, AbilitiesConfig abilitiesConfig, ModificationsConfig modificationsConfig)
    {
        _name = characterConfig.Name;
        Name = new ReactiveProperty<string>(_name);
        
        _hp = characterConfig.Hp;
        HP = new ReactiveProperty<int>(_hp);
        
        _armor = characterConfig.Armor;
        Armor = new ReactiveProperty<int>(_armor);
        
        _maxHp = _hp;
        _maxArmor = _armor;
        
        MaxHP = new ReactiveProperty<int>(_maxHp);
        MaxArmor = new ReactiveProperty<int>(_maxArmor);
        
        _avatar = characterConfig.Avatar;
        Avatar = new ReactiveProperty<Sprite>(_avatar);
        
        _partyIcon = characterConfig.PartyIcon;
        PartyIcon = new ReactiveProperty<Sprite>(_partyIcon);

        _abilities = new ObservableList<Ability>(abilitiesConfig.GetAbilities(characterConfig.AbilitySize));
        _modifications = new ObservableList<Modification>(modificationsConfig.GetModifications());

        HealthDisplay = HP.CombineLatest(MaxHP, (current, max) => $"{current}/{max}")
            .ToReadOnlyReactiveProperty();
    }
    
    public bool ApplyModificationToAbility(Ability ability, Modification modification)
    {
        if (!CanApplyModification(ability, modification)) return false;

        ability.AppliedModification = modification;
        _modifications.Remove(modification);
        
        OnAbilityModified?.Invoke(ability);
        return true;
    }

    public void RemoveModificationFromAbility(Ability ability)
    {
        var modification = ability.AppliedModification;
        if (modification == null) return;

        ability.AppliedModification = null;
        _modifications.Add(modification);

        OnAbilityModified?.Invoke(ability);
    }
    
    private bool CanApplyModification(Ability ability, Modification modification)
    {
        return ability.AppliedModification == null && ability.CompatibleModificationTypes.Contains(modification.Type);
    }
}