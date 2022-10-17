using UnityEngine;
using UniRx;

public abstract class BaseJumpLocomotion : BaseLocomotion
{
    protected readonly float _coyoteTime = 0.1f;

    public override LocomotionType Type => LocomotionType.Jump;
    protected float CoyoteTimeCounter { get; private set; }

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        CompositeDisposable coyoteTimeCounterDisposable = new CompositeDisposable();

        IsGrounded
            .Subscribe(state =>
            {
                if(state == true)
                {
                    coyoteTimeCounterDisposable.Clear();
                    CoyoteTimeCounter = _coyoteTime;
                }
                else
                {
                    Observable
                        .EveryUpdate()
                        .Subscribe(_ =>
                        {
                            CoyoteTimeCounter -= Time.deltaTime;
                            
                            if(CoyoteTimeCounter < 0)
                            {
                                CoyoteTimeCounter = 0;
                                coyoteTimeCounterDisposable.Clear();
                            }
                        })
                        .AddTo(coyoteTimeCounterDisposable);
                }
            })
            .AddTo(_disposable);
    }
}
