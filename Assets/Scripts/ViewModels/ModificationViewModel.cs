using System;
using R3;
using UnityEngine;

public class ModificationViewModel : IDisposable
{
    public ReactiveProperty<string> Name { get; }
    public ReactiveProperty<Sprite> Icon { get; }
    public ReactiveProperty<ModificationType> Type { get; }
    public ReactiveProperty<bool> IsApplied { get; }
    
    public Modification Model { get; }

    private readonly CompositeDisposable _disposables = new();

    public ModificationViewModel(Modification model)
    {
        Model = model;
        
        Name = new ReactiveProperty<string>(model.Name);
        Icon = new ReactiveProperty<Sprite>(model.Icon);
        Type = new ReactiveProperty<ModificationType>(model.Type);
        IsApplied = new ReactiveProperty<bool>(false);
    }
    
    public void SetApplied(bool isApplied)
    {
        IsApplied.Value = isApplied;
    }
    
    public void Dispose()
    {
        _disposables?.Dispose();
        Name?.Dispose();
        Icon?.Dispose();
        Type?.Dispose();
        IsApplied?.Dispose();
    }
}