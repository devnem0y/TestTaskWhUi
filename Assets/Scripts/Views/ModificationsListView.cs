using System.Collections;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ModificationsListView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private ModificationView _modificationPrefab;
    
    private CharacterViewModel _currentCharacterVM;
    private readonly CompositeDisposable _disposables = new();

    public void Init(CharacterViewModel characterViewModel, PlayerInput playerInput)
    {
        StartCoroutine(RefreshModifications(characterViewModel, playerInput));
        
        characterViewModel.AbilityViewModels
            .ForEach(abilityVM =>
                abilityVM.IsHovered.Subscribe(_ => SyncModificationCompatibilityStates())
                    .AddTo(_disposables));

        characterViewModel.DragDropService.IsDragging
            .Subscribe(isDragging => _scrollRect.enabled = !isDragging)
            .AddTo(_disposables);
    }
    
    private IEnumerator RefreshModifications(CharacterViewModel characterViewModel, PlayerInput playerInput)
    {
        foreach (Transform child in _content) Destroy(child.gameObject);

        _disposables.Clear();
        _currentCharacterVM = characterViewModel;
        
        yield return null;
        
        foreach (var modificationVM in characterViewModel.ModificationViewModels)
        {
            var modificationView = Instantiate(_modificationPrefab, _content);
            modificationView.Init(modificationVM, playerInput);
        }
    }
    
    private void SyncModificationCompatibilityStates()
    {
        foreach (var modVM in _currentCharacterVM.ModificationViewModels)
        {
            var hasCompatibleHover = _currentCharacterVM.AbilityViewModels
                .Any(abilityVM => abilityVM.IsHovered.CurrentValue 
                                  && abilityVM.Model.CanApplyModification(modVM.Model));
            
            modVM.UpdateCompatibilityWithAbility(hasCompatibleHover);
        }
    }
}