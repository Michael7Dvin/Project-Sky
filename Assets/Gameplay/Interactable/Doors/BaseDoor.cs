using UnityEngine;
using UniRx;

[RequireComponent(typeof(BaseLock))]
public abstract class BaseDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isInteractionActive = true;

    [Tooltip("Closing & Opening aren't allowed")]
    [SerializeField] private DoorState _initialState;

    protected readonly ReactiveProperty<DoorState> _state = new ReactiveProperty<DoorState>();

    private BaseLock _lock;

    public bool IsInteractionActive => _isInteractionActive;
    public IReadOnlyReactiveProperty<DoorState> State => _state;

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

    public abstract void Open();
    public abstract void Close();

    public void Interact()
    {
        if (_isInteractionActive == true)
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
}
