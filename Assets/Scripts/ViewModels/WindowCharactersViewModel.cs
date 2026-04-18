using System;
using ObservableCollections;
using R3;

public class WindowCharactersViewModel : IDisposable
{
    private readonly IObservableCollection<Character> _modelCharacters;
    private readonly ObservableList<CharacterViewModel> _characterViewModels;
    private readonly ReactiveProperty<CharacterViewModel> _selectedCharacterSubject;
    private readonly CompositeDisposable _disposables = new();

    public IObservableCollection<CharacterViewModel> Characters { get; }
    public ReactiveCommand<int> SelectCharacterCommand { get; }
    public ReadOnlyReactiveProperty<CharacterViewModel> SelectedCharacter { get; }

    public WindowCharactersViewModel(IObservableCollection<Character> modelCharacters)
    {
        _modelCharacters = modelCharacters;
        _characterViewModels = new ObservableList<CharacterViewModel>();
        Characters = _characterViewModels;
        
        _selectedCharacterSubject = new ReactiveProperty<CharacterViewModel>(null);
        SelectedCharacter = _selectedCharacterSubject.ToReadOnlyReactiveProperty();

        SelectCharacterCommand = new ReactiveCommand<int>();

        InitializeCollections();
        SetupReactiveBindings();
        
        if (_characterViewModels.Count > 0) SelectCharacterCommand.Execute(0);
    }

    private void InitializeCollections()
    {
        foreach (var character in _modelCharacters)
        {
            _characterViewModels.Add(new CharacterViewModel(character));
        }
    }

    private void SetupReactiveBindings()
    {
        SelectCharacterCommand
            .Subscribe(index =>
            {
                if (index >= 0 && index < _characterViewModels.Count) 
                    _selectedCharacterSubject.Value = _characterViewModels[index];
                else _selectedCharacterSubject.Value = null;
            })
            .AddTo(_disposables);
    }

    public void Dispose()
    {
        foreach (var characterVm in _characterViewModels) characterVm.Dispose();

        _disposables?.Dispose();
        _selectedCharacterSubject?.Dispose();
    }
}