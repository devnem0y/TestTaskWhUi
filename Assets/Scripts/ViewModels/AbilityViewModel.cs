using System;
using R3;
using UnityEngine;

public class AbilityViewModel : IDisposable
{
    public ReactiveProperty<string> Name { get; }
    public ReactiveProperty<Sprite> Icon { get; }
    public ReactiveProperty<ModificationType?> AppliedModificationType { get; }
    public ReactiveProperty<bool> IsModified { get; }
    
    public Ability Model { get; }

    private readonly CompositeDisposable _disposables = new();

    public AbilityViewModel(Ability model)
    {
        Model = model;
        
        Name = new ReactiveProperty<string>(model.Name);
        Icon = new ReactiveProperty<Sprite>(model.Icon);
        AppliedModificationType = new ReactiveProperty<ModificationType?>(model.GetAppliedModificationType());
        IsModified = new ReactiveProperty<bool>(model.AppliedModification != null);
        
        model.AppliedModificationReactive.Subscribe(modification =>
            {
                AppliedModificationType.Value = modification?.Type;
                IsModified.Value = modification != null;
            }).AddTo(_disposables);
    }

    public void Dispose()
    {
        _disposables?.Dispose();
        Name?.Dispose();
        Icon?.Dispose();
        AppliedModificationType?.Dispose();
        IsModified?.Dispose();
    }
}
