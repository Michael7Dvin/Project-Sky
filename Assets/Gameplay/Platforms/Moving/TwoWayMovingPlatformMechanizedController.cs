using UnityEngine;
using UniRx;

[RequireComponent(typeof(TwoWayMovingPlatform))]
public class TwoWayMovingPlatformMechanizedController : MonoBehaviour
{
    [SerializeField] private LogicalMechanism _moveToNextLogicalMechanism;
    [SerializeField] private LogicalMechanism _moveToPreviousLogicalMechanism;

    private TwoWayMovingPlatform _platform;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void OnValidate()
    {
        if (_moveToNextLogicalMechanism == null && _moveToPreviousLogicalMechanism == null)
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
        if (_moveToNextLogicalMechanism is not null)
        {
            _moveToNextLogicalMechanism
                .Output
                .Where(value => value == true)
                .Subscribe(value => _platform.TryMoveToNext())
                .AddTo(_disposable);
        }

        if (_moveToPreviousLogicalMechanism is not null)
        {
            _moveToPreviousLogicalMechanism
                .Output
                .Where(value => value == true)
                .Subscribe(value => _platform.TryMoveToPrevious())
                .AddTo(_disposable);
        }
    }

    private void OnDisable() => _disposable.Clear(); 
}
