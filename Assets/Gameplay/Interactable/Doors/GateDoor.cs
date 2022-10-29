using UnityEngine;
using DG.Tweening;

public class GateDoor : BaseDoor
{
    [SerializeField] private Transform _leftMovingPart;
    [SerializeField] private Transform _rightMovingPart;

    [SerializeField] private float _openingSpeed;
    [SerializeField] private float _closingSpeed;

    [SerializeField] private float _leftOpenedYRotation;
    [SerializeField] private float _rightOpenedYRotation;

    [SerializeField] private float _leftClosedYRotation;
    [SerializeField] private float _rightClosedYRotation;

    private Vector3 _leftInitialRotation;
    private Vector3 _rightInitialRotation;

    protected override void Awake()
    {
        base.Awake();

        _leftInitialRotation = _leftMovingPart.eulerAngles;
        _rightInitialRotation = _rightMovingPart.eulerAngles;
    }

    public override void Open()
    {
        base.Open();

        _state.Value = DoorState.Opening;

        bool leftOpened = false;
        bool rightOpened = false;

        GetRotation(_leftMovingPart, _leftOpenedYRotation, _openingSpeed, _leftInitialRotation)
            .Play()
            .OnComplete(() =>
            {
                leftOpened = true;
                TrySetStateByCheck();
            });

        GetRotation(_rightMovingPart, _rightOpenedYRotation, _openingSpeed, _rightInitialRotation)
            .Play()
            .OnComplete(() =>
            {
                rightOpened = true;
                TrySetStateByCheck();
            });
        
        void TrySetStateByCheck()
        {
            if (leftOpened == true && rightOpened == true)
            {
                _state.Value = DoorState.Opened;
            }
        }
    }

    public override void Close()
    {
        base.Close();

        _state.Value = DoorState.Closing;

        bool leftClosed = false;
        bool rightClosed = false;

        GetRotation(_leftMovingPart, _leftClosedYRotation, _closingSpeed, _leftInitialRotation)
            .Play()
            .OnComplete(() =>
            {
                leftClosed = true;
                TrySetStateByCheck();
            });

        GetRotation(_rightMovingPart, _rightClosedYRotation, _closingSpeed, _rightInitialRotation)
            .Play()
            .OnComplete(() =>
            {
                rightClosed = true;
                TrySetStateByCheck();
            });

        void TrySetStateByCheck()
        {
            if (leftClosed == true && rightClosed == true)
            {
                _state.Value = DoorState.Closed;
            }
        }
    }

    protected override void StopOpening() => DOTween.Kill(gameObject);
    protected override void StopClosing() => DOTween.Kill(gameObject);

    private Tween GetRotation(Transform transform, float yRotation, float speed, Vector3 _initialRotation)
    {
        return transform
            .DOLocalRotate(new Vector3(_initialRotation.x, yRotation, _initialRotation.z), speed, RotateMode.Fast)
            .SetEase(Ease.Flash)
            .SetSpeedBased()
            .SetUpdate(UpdateType.Normal)
            .SetLink(gameObject);
    }
}
