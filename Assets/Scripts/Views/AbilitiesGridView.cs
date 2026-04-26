using System.Collections;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitiesGridView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private AbilityView _abilityPrefab;

    private CharacterViewModel _currentCharacterVM;
    private readonly CompositeDisposable _disposables = new();

    public void Init(CharacterViewModel characterViewModel, PlayerInput playerInput)
    {
        StopAllCoroutines();
        
        StartCoroutine(RefreshAbilities(characterViewModel, playerInput));
        
        characterViewModel.ModificationViewModels
            .ForEach(modVM =>
                modVM.IsHovered.Subscribe(_ => SyncCompatibilityStates())
                    .AddTo(_disposables));
    }
    
    private IEnumerator RefreshAbilities(CharacterViewModel characterViewModel, PlayerInput playerInput)
    {
        foreach (Transform child in _content) Destroy(child.gameObject);

        _disposables.Clear();
        _currentCharacterVM = characterViewModel;

        yield return null;
        
        foreach (var abilityVM in characterViewModel.AbilityViewModels)
        {
            var abilityView = Instantiate(_abilityPrefab, _content);
            abilityView.Init(abilityVM, playerInput);
        }
    }
    
    private void SyncCompatibilityStates()
    {
        foreach (var abilityVM in _currentCharacterVM.AbilityViewModels)
        {
            var hasCompatibleHover = _currentCharacterVM.ModificationViewModels
                .Any(modVM => modVM.IsHovered.CurrentValue 
                              && abilityVM.Model.CanApplyModification(modVM.Model));

            abilityVM.HasCompatibleHoveredModification.Value = hasCompatibleHover;
        }
    }
}