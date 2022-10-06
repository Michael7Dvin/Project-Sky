using UnityEngine;
using UniRx;

public abstract class LocomotionInput : MonoBehaviour
{
    protected readonly ReactiveProperty<LocomotionType> _currentLocomotionType = new ReactiveProperty<LocomotionType>();
    protected readonly ReactiveProperty<(LocomotionMoveSpeedType, bool)> _locomotionMoveSpeedTypeAction = new ReactiveProperty<(LocomotionMoveSpeedType, bool)>();

    public Vector3 Direction { get; protected set; }
    public float Magnitude => Mathf.Clamp01(Direction.magnitude);

    public IReadOnlyReactiveProperty<LocomotionType> CurrentLocomotionType => _currentLocomotionType;
    public IReadOnlyReactiveProperty<(LocomotionMoveSpeedType, bool)> LocomotionMoveSpeedTypeAction => _locomotionMoveSpeedTypeAction;
}
