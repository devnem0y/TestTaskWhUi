using System;
using ObservableCollections;
using R3;
using UnityEngine;

public class CharacterViewModel : IDisposable
{
    private readonly Character _model;
    private readonly CompositeDisposable _disposables = new();
    
    public string Name => _model.Name;
    public int HP => _model.HP;
    public int MaxHP => _model.MaxHP;
    public int Armor => _model.Armor;
    public int MaxArmor => _model.MaxArmor;
    public Sprite Portrait => _model.Portrait;
    public Sprite PartyIcon => _model.PartyIcon;

    public DragDropService DragDropService { get; }

    public ObservableList<IAbilityViewModel> AbilityViewModels { get; } = new();
    public ObservableList<IModificationViewModel> ModificationViewModels { get; } = new();

    public CharacterViewModel(Character model)
    {
        _model = model;
        DragDropService = new DragDropService();
        
        foreach (var ability in _model.Abilities)
        {
            AbilityViewModels.Add(new AbilityViewModel(ability, DragDropService));
        }

        foreach (var modification in _model.Modifications)
        {
            ModificationViewModels.Add(new ModificationViewModel(modification, DragDropService));
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}