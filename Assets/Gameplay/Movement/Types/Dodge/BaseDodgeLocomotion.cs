using UniRx;

public abstract class BaseDodgeLocomotion : BaseLocomotion
{
    protected readonly float _rollDuration;

    public readonly ReactiveCommand RollCompleted = new ReactiveCommand();

    protected BaseDodgeLocomotion(float rollDuration)
    {
        _rollDuration = rollDuration;
    }

    public float RollDuration => _rollDuration;
    public override LocomotionType Type => LocomotionType.Dodge;
}
