using R3;
using UnityEngine;

public class WindowCharactersView : MonoBehaviour
{
    [SerializeField] private PartyCharacterView _partyCharacterPrefab;
    [SerializeField] private PartyView _partyView;
    [SerializeField] private CharacterInfoView _characterInfoView;

    private WindowCharactersViewModel _viewModel;
    
    private readonly CompositeDisposable _disposables = new ();

    public void Init(WindowCharactersViewModel viewModel)
    {
        _viewModel = viewModel;

        _partyView.Init(_viewModel.Characters, _partyCharacterPrefab);
        _partyView.onCharacterSelected += OnCharacterSelected;

        // Подписываемся на изменения выбранного персонажа
        _viewModel.SelectedCharacter
            .Subscribe(characterVm =>
            {
                _characterInfoView.UpdateView(characterVm);
            })
            .AddTo(_disposables);
    }

    private void OnCharacterSelected(int index)
    {
        _viewModel.SelectCharacterCommand.Execute(index);
    }

    private void OnDestroy()
    {
        _partyView.onCharacterSelected -= OnCharacterSelected;
        _disposables?.Dispose();
    }
}