using System;
using System.Collections;
using System.Linq;
using ObservableCollections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyView : MonoBehaviour
{
    [SerializeField] private PartySizeController _sizeController;
    [SerializeField] private Transform _content;
    [SerializeField] private Transform _selectionFrame;
    [SerializeField] private PlayerInput _playerInput;
    
    public event Action<int> onCharacterSelected;

    public void Init(IObservableCollection<CharacterViewModel> characterViewModels, PartyCharacterView characterViewPrefab)
    {
        for (var i = 0; i < characterViewModels.Count; i++)
        {
            var characterVm = characterViewModels.ElementAt(i);
            var partyView = Instantiate(characterViewPrefab, _content);
            partyView.Init(i, characterVm.PartyIcon, _playerInput);

            partyView.onSelected += value =>
            {
                SetSelectionFrame(value);
                onCharacterSelected?.Invoke(value);
            };
        }

        StartCoroutine(RebuildUi());
    }
    
    private void SetSelectionFrame(int selectedCharacterIndex)
    {
        _selectionFrame.position = _content.GetChild(selectedCharacterIndex).position;
        _selectionFrame.gameObject.SetActive(true);
    }
    
    private IEnumerator RebuildUi()
    {
        yield return new WaitForEndOfFrame();
        _sizeController.Rebuild();
        SetSelectionFrame(0);
    }
}