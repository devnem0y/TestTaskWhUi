using R3;
using UnityEngine.InputSystem;

public abstract class ViewBase<T> : PointerBehaviour where T : class
{
    protected T ViewModel { get; private set; }
    protected readonly CompositeDisposable _viewDisposables = new();

    public virtual void Init(T viewModel, PlayerInput playerInput)
    {
        ViewModel = viewModel;
        OnViewModelSet();
        SetupInputSystem(playerInput);
    }

    protected virtual void OnViewModelSet() { }

    private void OnDestroy()
    {
        CleanupInputSystem();
        _viewDisposables.Dispose();
    }
}