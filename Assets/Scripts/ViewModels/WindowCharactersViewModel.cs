using System;
using System.Collections.Specialized;
using System.Linq;
using ObservableCollections;
using R3;

public class WindowCharactersViewModel : IDisposable
{
    private Character _selectedCharacter;
    
    private readonly IObservableCollection<Character> _modelCharacters;
    private readonly ObservableList<CharacterViewModel> _characterViewModels;
    private readonly CompositeDisposable _disposables = new();
    
    public event Action<Character> SelectedCharacterChanged;
    
    public IObservableCollection<CharacterViewModel> Characters { get; }
    public ReactiveCommand<Character> SelectCharacterCommand { get; set; }
    
    public Character SelectedCharacter
    {
        get => _selectedCharacter;
        set
        {
            if (_selectedCharacter == value) return;
            _selectedCharacter = value;
            SelectedCharacterChanged?.Invoke(value);
        }
    }

    public WindowCharactersViewModel(IObservableCollection<Character> modelCharacters)
    {
        _modelCharacters = modelCharacters;
        _characterViewModels = new ObservableList<CharacterViewModel>();
        Characters = _characterViewModels;

        InitializeCollections();
        SubscribeToCollectionChanges();
    }

    private void InitializeCollections()
    {
        foreach (var character in _modelCharacters)
        {
            _characterViewModels.Add(new CharacterViewModel(character));
        }
    }

    private void SubscribeToCollectionChanges()
    {
        _modelCharacters.CollectionChanged += OnCharactersCollectionChanged;
    }

    private void OnCharactersCollectionChanged(in NotifyCollectionChangedEventArgs<Character> e)
    {
        UpdateCharacterViewModels(e);
    }

    private void UpdateCharacterViewModels(in NotifyCollectionChangedEventArgs<Character> e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var character in e.NewItems)
                {
                    _characterViewModels.Add(new CharacterViewModel(character));
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var character in e.OldItems)
                {
                    var vmToRemove = _characterViewModels.FirstOrDefault(vm => vm.Model == character);
                    if (vmToRemove == null) continue;
                    vmToRemove.Dispose();
                    _characterViewModels.Remove(vmToRemove);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                foreach (var vm in _characterViewModels) vm.Dispose();
                _characterViewModels.Clear();
                InitializeCollections();
                break;
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Replace:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void InitializeCommands()
    {
        SelectCharacterCommand = new ReactiveCommand<Character>();
        SelectCharacterCommand.Subscribe(character => { SelectedCharacter = character; }).AddTo(_disposables);
    }

    public void Dispose()
    {
        _modelCharacters.CollectionChanged -= OnCharactersCollectionChanged;
        
        foreach (var characterVm in _characterViewModels) characterVm.Dispose();

        _disposables?.Dispose();
    }
}