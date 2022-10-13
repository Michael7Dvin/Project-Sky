using UnityEngine;

public class TeleportDodgeLocomotion : BaseDodgeLocomotion
{
    public TeleportDodgeLocomotion(float rollDuration) : base(rollDuration)
    {
    }

    public override float VerticalMoveSpeed => throw new System.NotImplementedException();

    public override float HorizontalMoveSpeed => throw new System.NotImplementedException();
}
