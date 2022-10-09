using UniRx;

public class MultiJumpLocomotion : DefaultJumpLocomotion
{
    private readonly int _additionalJumps;
    private int _remainingJumps;

    public MultiJumpLocomotion(float jumpSpeed, float normalHorizontalSpeed, float sprintHorizontalSpeed, int additionalJumps) : base(jumpSpeed, normalHorizontalSpeed, sprintHorizontalSpeed)
    {
        _additionalJumps = additionalJumps;
        _remainingJumps = additionalJumps;
    }

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        LocomotionComposition
           .CurrentLocomotionType
           .Subscribe(type =>
           {
               if (type == LocomotionType.Jump)
               {
                   if (LocomotionComposition.GroundDetector.IsGrounded.Value == false)
                   {
                       if (_remainingJumps > 0)
                       {
                           Jump();
                           _remainingJumps--;
                       }
                   }
               }
               else if(type != LocomotionType.Fall)
               {
                   _remainingJumps = _additionalJumps;
               }
           })
           .AddTo(_disposable);
    }
}
