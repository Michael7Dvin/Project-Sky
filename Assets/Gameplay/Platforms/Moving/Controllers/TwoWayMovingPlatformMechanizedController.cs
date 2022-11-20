using UnityEngine;
using UniRx;

[RequireComponent(typeof(TwoWayMovingPlatform))]
public class TwoWayMovingPlatformMechanizedController : MonoBehaviour
{
    [SerializeField] private LogicalMechanism _moveToNextMechanism;
    [SerializeField] private LogicalMechanism _moveToPreviousMechanism;

    private TwoWayMovingPlatform _platform;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void OnValidate()
    {
        if (_moveToNextMechanism == null && _moveToPreviousMechanism == null)
        {
            Debug.LogError($"{gameObject} Mechanized Controller should have at least one logical mechanism applied");
        }
    }

    private void Awake()
    {
        _platform = GetComponent<TwoWayMovingPlatform>();
    }

    private void OnEnable()
    {
        if (_moveToNextMechanism is not null)
        {
            _moveToNextMechanism
                .ReadOnlyOutput
                .Where(value => value == true)
                .Subscribe(value => _platform.TryMoveToNext())
                .AddTo(_disposable);
        }

        if (_moveToPreviousMechanism is not null)
        {
            _moveToPreviousMechanism
                .ReadOnlyOutput
                .Where(value => value == true)
                .Subscribe(value => _platform.TryMoveToPrevious())
                .AddTo(_disposable);
        }
    }

    private void OnDisable() => _disposable.Clear(); 
}
