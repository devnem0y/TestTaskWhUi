using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class WindowCharactersView : MonoBehaviour
{
    [SerializeField] private PartyView _partyView;
    [SerializeField] private CharacterInfoView _characterInfoView;
    [SerializeField] private ModificationsListView _modificationsListView;
    [SerializeField] private AbilitiesGridView _abilitiesGridView;
    [SerializeField] private ModificationPreviewView _modificationPreviewPrefab;
    [SerializeField] private PlayerInput _playerInput;

    private CharacterViewModel _currentCharacter;
    private string _currentModificationId;
    private WindowCharactersViewModel _viewModel;
    private readonly CompositeDisposable _disposables = new();
    
    private InputAction _keyboardActionSubmit;
    private InputAction _keyboardActionCancel;

    public void Init(WindowCharactersViewModel viewModel)
    {
        _viewModel = viewModel;
        SetupPartySelection();
        SetupInputSystem(_playerInput); // TODO: Esc - Exit | Enter - Restart
        
        _viewModel.SelectedCharacterViewModel
            .Subscribe(characterVM =>
            {
                _disposables.Clear();
                
                _characterInfoView.Init(characterVM);
                _abilitiesGridView.Init(characterVM, _playerInput);
                _modificationsListView.Init(characterVM, _playerInput);
                _partyView.UpdateSelection(characterVM);
                
                SetupDragTrackingForCharacter(characterVM);
            }).AddTo(this);
    }

    private void SetupDragTrackingForCharacter(CharacterViewModel characterVM)
    {
        characterVM?.DragDropService.IsDragging
            .Subscribe(isDragging =>
            {
                if (!isDragging) return;

                var currentModification = characterVM.DragDropService.DraggedModification.CurrentValue;
                var modificationPreview = Instantiate(_modificationPreviewPrefab, transform);
                modificationPreview.Init(currentModification, _playerInput);
            }).AddTo(_disposables);
    }

    private void SetupPartySelection()
    {
        _partyView.Init(_viewModel.CharacterViewModels, _playerInput);

        _partyView.OnCharacterSelected
            .Subscribe(selectedCharacterVM =>
            {
                _viewModel.SelectedCharacterViewModel.Value = selectedCharacterVM;
            }).AddTo(this);
    }
    
    private void OnDestroy()
    {
        _keyboardActionSubmit.performed -= Restart;
        _keyboardActionCancel.performed -= Exit;
        
        _disposables.Dispose();
        _viewModel.Dispose();
    }

    #region Input

    private void SetupInputSystem(PlayerInput playerInput)
    {
        _keyboardActionSubmit = playerInput.actions["Submit"];
        _keyboardActionSubmit.performed += Restart;
        
        _keyboardActionCancel = playerInput.actions["Cancel"];
        _keyboardActionCancel.performed += Exit;
    }

    private static void Restart(InputAction.CallbackContext context)
    {
        SceneManager.LoadSceneAsync(0);
    }

    private static void Exit(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion
}