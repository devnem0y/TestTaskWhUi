using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;

public class WindowCharactersViewModel
{
    public ObservableList<CharacterViewModel> CharacterViewModels { get; } = new();
    public ReactiveProperty<CharacterViewModel> SelectedCharacterViewModel { get; } = new();

    private readonly CompositeDisposable _disposables = new();
    
    private readonly List<Character> _characters;
    
    public WindowCharactersViewModel(List<Character> characters)
    {
        foreach (var character in characters) CharacterViewModels.Add(new CharacterViewModel(character));

        SelectedCharacterViewModel.Value = CharacterViewModels.FirstOrDefault();
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}