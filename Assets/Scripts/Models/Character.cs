using System;
using ObservableCollections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Character : ICharacter
{
    private readonly ObservableList<Ability> _abilities;
    private readonly ObservableList<Modification> _modifications;
    
    public string Name { get; }
    public int HP { get; }
    public int MaxHP { get; }
    public int Armor { get; }
    public int MaxArmor { get; }
    public Sprite PartyIcon { get; }
    public Sprite Portrait { get; }
    
    public IObservableCollection<Ability> Abilities => _abilities;
    public IObservableCollection<Modification> Modifications => _modifications;
    
    public event Action<Ability> OnAbilityModified;
    
    public Character(CharacterConfig characterConfig, AbilitiesConfig abilitiesConfig, ModificationsConfig modificationsConfig)
    {
        Name = characterConfig.Name;
        MaxHP = characterConfig.Hp;
        MaxArmor = characterConfig.Armor;
        Portrait = characterConfig.Portrait;
        PartyIcon = characterConfig.PartyIcon;
        
        _abilities = new ObservableList<Ability>(abilitiesConfig.GetAbilities(characterConfig.AbilitySize));
        _modifications = new ObservableList<Modification>(modificationsConfig.GetModifications());
        
        //TODO: Случайные значени для иметации данных
        HP = Random.Range(1, MaxHP);
        Armor = Random.Range(1, MaxArmor);
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