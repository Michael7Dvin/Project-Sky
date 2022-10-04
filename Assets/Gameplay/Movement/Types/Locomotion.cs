using UnityEngine;

public abstract class Locomotion
{
    public abstract LocomotionType Type { get; }
    public abstract Vector3 Velocity { get; }

    public abstract float VerticalMoveSpeed { get; }
    public abstract float HorizontalMoveSpeed { get; }

    protected LocomotionComposition LocomotionComposition { get; private set; }
    protected LocomotionInput Input => LocomotionComposition.Input;
    protected CharacterController CharacterController => LocomotionComposition.CharacterController;
    protected Transform Transform => LocomotionComposition.transform;

    public virtual void Initialize(LocomotionComposition locomotionComposition)
    {
        LocomotionComposition = locomotionComposition;
    }
    public abstract void Disable();
}
