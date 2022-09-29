using UnityEngine;
using UniRx;

public abstract class LocomotionInput : MonoBehaviour
{
    protected readonly ReactiveProperty<(LocomotionState, bool)> _locomotionInputAction = new ReactiveProperty<(LocomotionState, bool)>();

    public Vector3 InputDirection { get; protected set; }
    public float InputMagnitude => Mathf.Clamp01(InputDirection.magnitude);
    public IReadOnlyReactiveProperty<(LocomotionState, bool)> LocomotionInputAction => _locomotionInputAction;
}
