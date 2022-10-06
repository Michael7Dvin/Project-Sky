using UnityEngine;

public class NoneGroundLocomotion : BaseGroundLocomotion
{
    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed => 0f;

    public override void Disable()
    {       
    }
}
