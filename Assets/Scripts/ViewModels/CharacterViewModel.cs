using System;
using System.Collections.Specialized;
using System.Linq;
using ObservableCollections;
using R3;
using UnityEngine;

public class CharacterViewModel : IDisposable
{
    private readonly Character _model;
    
    public ReadOnlyReactiveProperty<string> Name { get; }
    public ReadOnlyReactiveProperty<int> HP { get; }
    public ReadOnlyReactiveProperty<int> MaxHP { get; }
    public ReadOnlyReactiveProperty<int> Armor { get; }
    public ReadOnlyReactiveProperty<int> MaxArmor { get; }
    public ReadOnlyReactiveProperty<Sprite> PartyIcon { get; }
    public ReadOnlyReactiveProperty<Sprite> Avatar { get; }
    public ReadOnlyReactiveProperty<string> HealthDisplay { get; }
    
    public IObservableCollection<AbilityViewModel> Abilities { get; }
    public IObservableCollection<ModificationViewModel> Modifications { get; }

    private readonly CompositeDisposable _disposables = new ();
    private readonly ObservableList<AbilityViewModel> _abilityViewModels;
    private readonly ObservableList<ModificationViewModel> _modificationViewModels;

    public CharacterViewModel(Character model)
    {
        _model = model;
        
        Name = _model.Name;
        HP = _model.HP;
        MaxHP = _model.MaxHP;
        Armor = _model.Armor;
        MaxArmor = _model.MaxArmor;
        PartyIcon = _model.PartyIcon;
        Avatar = _model.Avatar;
        HealthDisplay = _model.HealthDisplay;
        
        _abilityViewModels = new ObservableList<AbilityViewModel>();
        _modificationViewModels = new ObservableList<ModificationViewModel>();

        Abilities = _abilityViewModels;
        Modifications = _modificationViewModels;
        
        InitializeCollections();
        SubscribeToCollectionChanges();
    }

    private void InitializeCollections()
    {
        foreach (var ability in _model.Abilities)
        {
            _abilityViewModels.Add(new AbilityViewModel(ability));
        }

        foreach (var modification in _model.Modifications)
        {
            _modificationViewModels.Add(new ModificationViewModel(modification));
        }
    }

    private void SubscribeToCollectionChanges()
    {
        _model.Abilities.CollectionChanged += OnAbilitiesCollectionChanged;
        _model.Modifications.CollectionChanged += OnModificationsCollectionChanged;
    }
    
    private void OnAbilitiesCollectionChanged(in NotifyCollectionChangedEventArgs<Ability> e)
    {
        UpdateAbilityViewModels(e);
    }
    
    private void OnModificationsCollectionChanged(in NotifyCollectionChangedEventArgs<Modification> e)
    {
        UpdateModificationViewModels(e);
    }

    
    private void UpdateAbilityViewModels(NotifyCollectionChangedEventArgs<Ability> e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var ability in e.NewItems)
                {
                    _abilityViewModels.Add(new AbilityViewModel(ability));
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var ability in e.OldItems)
                {
                    var vmToRemove = _abilityViewModels.FirstOrDefault(vm => vm.Model == ability);
                    if (vmToRemove == null) continue;
                    vmToRemove.Dispose();
                    _abilityViewModels.Remove(vmToRemove);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                foreach (var vm in _abilityViewModels)
                {
                    vm.Dispose();
                }
                _abilityViewModels.Clear();
                InitializeCollections();
                break;
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Replace:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateModificationViewModels(NotifyCollectionChangedEventArgs<Modification> e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var modification in e.NewItems)
                {
                    _modificationViewModels.Add(new ModificationViewModel(modification));
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var modification in e.OldItems)
                {
                    var vmToRemove = _modificationViewModels.FirstOrDefault(vm => vm.Model == modification);
                    if (vmToRemove == null) continue;
                    vmToRemove.Dispose();
                    _modificationViewModels.Remove(vmToRemove);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                foreach (var vm in _modificationViewModels)
                {
                    vm.Dispose();
                }
                _modificationViewModels.Clear();
                InitializeCollections();
                break;
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Replace:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Dispose()
    {
        _model.Abilities.CollectionChanged -= OnAbilitiesCollectionChanged;
        _model.Modifications.CollectionChanged -= OnModificationsCollectionChanged;
        
        foreach (var abilityVm in _abilityViewModels) abilityVm.Dispose();
        foreach (var modificationVm in _modificationViewModels) modificationVm.Dispose();

        _disposables?.Dispose();
    }
}