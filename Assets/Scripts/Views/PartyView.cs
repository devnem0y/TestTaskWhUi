using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartyView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private PartyCharacterView _characterViewPrefab;
    [SerializeField] private Transform _selectionFrame;
    
    public readonly Subject<CharacterViewModel> OnCharacterSelected = new();

    private ObservableList<CharacterViewModel> _characterViewModels;
    private readonly CompositeDisposable _disposables = new();
    private readonly Dictionary<CharacterViewModel, PartyCharacterView> _characterViews = new();

    public void Init(ObservableList<CharacterViewModel> characterViewModels, PlayerInput playerInput)
    {
        _characterViewModels = characterViewModels;
        CreateCharacterIcons(playerInput);
    }
    
    public void UpdateSelection(CharacterViewModel selectedCharacterVM)
    {
        if (selectedCharacterVM != null && _characterViews.TryGetValue(selectedCharacterVM, out var characterView))
        {
            var targetPosition = characterView.transform.position;
            SetSelectionFrame(targetPosition);
        }
        else if (_selectionFrame != null) _selectionFrame.gameObject.SetActive(false);
    }

    private void CreateCharacterIcons(PlayerInput playerInput)
    {
        foreach (Transform child in _content) Destroy(child.gameObject);

        _disposables.Clear();
        _characterViews.Clear();
        
        foreach (var characterVM in _characterViewModels)
        {
            var characterView = Instantiate(_characterViewPrefab, _content);
            characterView.SetCharacterViewModel(characterVM, playerInput);
            
            _characterViews[characterVM] = characterView;
            
            characterView.OnClicked.Subscribe(_ => 
                OnCharacterSelected.OnNext(characterVM)).AddTo(_disposables);
        }
        
        StartCoroutine(SequencedCall());
    }
    
    private IEnumerator SequencedCall()
    {
        yield return null;
        CustomSizeUtils.RebuildWidth(_layoutElement, _content.GetComponent<RectTransform>(), 20f);
        UpdateSelection(_characterViewModels.FirstOrDefault());
    }
    
    private void SetSelectionFrame(Vector3 position)
    {
        if (_selectionFrame == null) return;
        
        _selectionFrame.position = position;
        _selectionFrame.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}