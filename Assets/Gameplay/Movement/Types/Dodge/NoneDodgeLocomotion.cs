public class NoneDodgeLocomotion : BaseDodgeLocomotion
{
    public NoneDodgeLocomotion(float rollDuration) : base(rollDuration)
    {
    }

    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed => 0f;
}
