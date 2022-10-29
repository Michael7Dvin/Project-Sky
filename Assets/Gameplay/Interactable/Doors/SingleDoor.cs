using UnityEngine;
using DG.Tweening;

public class SingleDoor : BaseDoor
{
    [SerializeField] private Transform _movingPart;

    [SerializeField] private float _openingSpeed;
    [SerializeField] private float _closingSpeed;

    [SerializeField] private float _openedYRotation;
    [SerializeField] private float _closedYRotation;

    private Vector3 _initialRotation;

    protected override void Awake()
    {
        base.Awake();

        _initialRotation = _movingPart.eulerAngles;
    }

    public override void Open()
    {
        base.Open();

        _state.Value = DoorState.Opening;

        GetRotation(_openedYRotation, _openingSpeed)
            .Play()
            .OnComplete(() => _state.Value = DoorState.Opened);
    }

    public override void Close()
    {
        base.Close();

        _state.Value = DoorState.Closing;

        GetRotation(_closedYRotation, _closingSpeed)
            .Play()
            .OnComplete(() => _state.Value = DoorState.Closed);
    }

    protected override void StopOpening() => DOTween.Kill(gameObject);
    protected override void StopClosing() => DOTween.Kill(gameObject);

    private Tween GetRotation(float yRotation, float speed)
    {
        return _movingPart
            .DOLocalRotate(new Vector3(_initialRotation.x, yRotation, _initialRotation.z), speed, RotateMode.Fast)
            .SetEase(Ease.Flash)
            .SetSpeedBased()
            .SetUpdate(UpdateType.Normal)
            .SetLink(gameObject);
    }

}
