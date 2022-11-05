using UnityEngine;
using UniRx;

[RequireComponent(typeof(BaseLock))]
public abstract class BaseDoor : MonoBehaviour, IInteractable
{
    protected readonly ReactiveProperty<DoorState> _state = new ReactiveProperty<DoorState>();

    [SerializeField] private bool _isInteractionAllowed = true;

    [Tooltip("Closing & Opening aren't allowed")]
    [SerializeField] private DoorState _initialState;

    private BaseLock _lock;

    public IReadOnlyReactiveProperty<DoorState> State => _state;
    public bool IsInteractionAllowed => _isInteractionAllowed;

    protected virtual void Awake()
    {
        if (_initialState == DoorState.Closing || _initialState == DoorState.Opening)
        {
            _state.Value = DoorState.Closed;
            Debug.LogWarning($"{gameObject} Closing & Opening aren't allowed as initialState value");
        }
        else
        {
            _state.Value = _initialState;
        }

        _lock = GetComponent<BaseLock>();
    }

    public virtual void Open() => StopClosing();
    public virtual void Close() => StopOpening();

    public void Interact()
    {
        if (_isInteractionAllowed == true)
        {
            if (_lock.IsLocked == false)
            {
                switch (_state.Value)
                {
                    case DoorState.Closed:
                        Open();
                        break;
                    case DoorState.Opened:
                        Close();
                        break;
                }
            }
        }
    }

    protected abstract void StopOpening();
    protected abstract void StopClosing();
}
