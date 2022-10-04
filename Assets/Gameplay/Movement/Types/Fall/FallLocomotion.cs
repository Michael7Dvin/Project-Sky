using UnityEngine;

public abstract class FallLocomotion : Locomotion
{
    public FallLocomotion(float verticalSpeed, float horizontalSpeed, float rotationSpeed)
    {
        VerticalSpeed = verticalSpeed;
        HorizontalSpeed = horizontalSpeed;
        RotationSpeed = rotationSpeed;
    }

    public float VerticalSpeed { get; private set; }
    public float HorizontalSpeed { get; private set; }
    public float RotationSpeed { get; private set; }

    public override LocomotionType Type => LocomotionType.Fall;
    public override abstract Vector3 Velocity { get; }
    public override abstract float HorizontalMoveSpeed { get; } 

    public override abstract void Disable();

    protected abstract void VerticalMove();
    protected abstract void HorizontalMove();
}
