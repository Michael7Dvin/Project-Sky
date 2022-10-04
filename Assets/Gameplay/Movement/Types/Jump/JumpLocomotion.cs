using UnityEngine;

public class JumpLocomotion : Locomotion
{
    public override LocomotionType Type => LocomotionType.Jump;
    public override Vector3 Velocity => throw new System.NotImplementedException();

    public override float VerticalMoveSpeed => throw new System.NotImplementedException();
    public override float HorizontalMoveSpeed => throw new System.NotImplementedException();

    public override void Disable()
    {
        throw new System.NotImplementedException();
    }
}
