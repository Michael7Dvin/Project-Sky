using System.Collections;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(TwoWayMovingPlatform))]
public class TwoWayMovingPlatformAutomaticController : MonoBehaviour
{
    private enum MoveDirection
    {
        ToLastMovePoint,
        ToFirstMovePoint,
    }

    [SerializeField] private MoveDirection _moveDirection;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _stopTime;
    private WaitForSeconds _waitStopTime;

    private TwoWayMovingPlatform _platform;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _waitStopTime = new WaitForSeconds(_stopTime);
        _platform = GetComponent<TwoWayMovingPlatform>();

        StartCoroutine(Move());
    }

    private void OnEnable()
    {
        _platform
            .MovementCompleted
            .Subscribe(_ => StartCoroutine(Move()))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
    
    private IEnumerator Move()
    {
        yield return _waitStopTime;

        if (_moveDirection == MoveDirection.ToLastMovePoint)
        {
            if (_platform.TryMoveToNext() == false)
            {
                _moveDirection = MoveDirection.ToFirstMovePoint;
                _platform.TryMoveToPrevious();
            }
        }
        else if (_moveDirection == MoveDirection.ToFirstMovePoint)
        {
            if (_platform.TryMoveToPrevious() == false)
            {
                _moveDirection = MoveDirection.ToLastMovePoint;
                _platform.TryMoveToNext();
            }
        }
    }
}
