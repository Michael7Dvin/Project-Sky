using UnityEngine;

public abstract class GroundLocomotion : Locomotion
{
    public GroundLocomotion(float jogSpeed, float sprintSpeed, float sneakSpeed, float rotationSpeed)
    {
        JogSpeed = jogSpeed;
        SprintSpeed = sprintSpeed;
        SneakSpeed = sneakSpeed;
        RotationSpeed = rotationSpeed;
    }

    public float JogSpeed { get; private set; }
    public float SprintSpeed { get; private set; }
    public float SneakSpeed { get; private set; }
    public float RotationSpeed { get; private set; }
    
    public override LocomotionType Type => LocomotionType.Ground;
    public override abstract Vector3 Velocity { get; } 
    public override abstract float HorizontalMoveSpeed { get; }

    public override abstract void Disable();

    protected abstract void Move();
}
