using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TwoWayMovingPlatform : MovingPlatform
{
    [SerializeField] private List<Transform> _movePoints;

    [SerializeField] private Transform _currentMovePoint;
    private Transform _nextMovePoint;
    private Transform _previousMovePoint;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void OnValidate()
    {
        if (_movePoints == null || _movePoints.Count < 2)
        {
            Debug.LogError($"{gameObject} platform should have at least 2 move points");
            return;
        }

        if (_movePoints.Contains(_currentMovePoint) == false)
        {
            Debug.LogError($"{gameObject} MovePoints should contain CurrentMovePoint");
        }
        else
        {
            if (transform.position != _currentMovePoint.transform.position)
            {
                transform.position = _currentMovePoint.transform.position;
            }

            SetMovePoints(_currentMovePoint);
        }
    }

    private void OnEnable()
    {
        MovementCompleted
            .Subscribe(movePoint => SetMovePoints(movePoint))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();

    public bool TryMoveToNext()
    {
        if (_nextMovePoint is not null)
        {
            _previousMovePoint = _currentMovePoint;
            _currentMovePoint = null;
            Move(_nextMovePoint);

            return true;
        }

        return false;
    }

    public bool TryMoveToPrevious()
    {
        if (_previousMovePoint is not null)
        {
            _nextMovePoint = _currentMovePoint;
            _currentMovePoint = null;
            Move(_previousMovePoint);

            return true;
        }

        return false;
    }

    private void SetMovePoints(Transform newCurrentMovePoint)
    {
        _currentMovePoint = newCurrentMovePoint;

        int currentMovePointIndex = _movePoints.IndexOf(newCurrentMovePoint);

        if (currentMovePointIndex == _movePoints.Count - 1)
        {
            _nextMovePoint = null;
            _previousMovePoint = _movePoints[currentMovePointIndex - 1];
        }
        else if (currentMovePointIndex == 0)
        {
            _nextMovePoint = _movePoints[currentMovePointIndex + 1];
            _previousMovePoint = null;
        }
        else
        {
            _nextMovePoint = _movePoints[currentMovePointIndex + 1];
            _previousMovePoint = _movePoints[currentMovePointIndex - 1];
        }
    }
}
