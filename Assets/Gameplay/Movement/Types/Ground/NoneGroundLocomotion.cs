using UnityEngine;

public class NoneGroundLocomotion : GroundLocomotion
{
    public NoneGroundLocomotion(float runSpeed, float sprintSpeed, float sneakSpeed, float rotationSpeed) : base(runSpeed, sprintSpeed, sneakSpeed, rotationSpeed)
    {
    }

    public override Vector3 Velocity => CharacterController.velocity;

    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed => 0f;

    public override void Disable()
    {       
    }

    protected override void Move()
    {
    }
}
