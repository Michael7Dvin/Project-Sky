using UnityEngine;
using DG.Tweening;

public class PortcullisDoor : BaseDoor
{
    [SerializeField] private Transform _movingPart;

    [SerializeField] private float _openingSpeed;
    [SerializeField] private float _closingSpeed;

    [SerializeField] private float _openedYPosition;
    [SerializeField] private float _closedYPosition;

    public override void Open()
    {
        base.Open();

        _state.Value = DoorState.Opening;

        GetMovement(_openedYPosition, _openingSpeed)
            .Play()
            .OnComplete(() => _state.Value = DoorState.Opened);
    }

    public override void Close()
    {
        base.Close();

        _state.Value = DoorState.Closing;

        GetMovement(_closedYPosition, _closingSpeed)
            .Play()
            .OnComplete(() => _state.Value = DoorState.Closed);
    }

    protected override void StopOpening() => DOTween.Kill(gameObject);
    protected override void StopClosing() => DOTween.Kill(gameObject);

    private Tween GetMovement(float yPosition, float speed)
    {
        return _movingPart
            .DOLocalMoveY(yPosition, speed)
            .SetEase(Ease.Flash)
            .SetSpeedBased()
            .SetUpdate(UpdateType.Normal)
            .SetLink(gameObject);
    }
}
