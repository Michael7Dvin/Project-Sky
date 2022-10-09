using UnityEngine;
using UniRx;

public class DefaultFallLocomotion : BaseFallLocomotion
{
    private readonly float _verticalSpeed;
    private readonly float _horizontalSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _verticalMoveDisposable = new CompositeDisposable();

    public DefaultFallLocomotion(float verticalSpeed, float horizontalSpeed) 
    {
        _verticalSpeed = verticalSpeed;
        _horizontalSpeed = horizontalSpeed;
    }

    public override float VerticalMoveSpeed => _verticalSpeed;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * _horizontalSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        StartVerticalMove();

        LocomotionComposition
            .CurrentLocomotionType
            .Subscribe(type =>
            {
                _verticalMoveDisposable.Clear();

                if(type != LocomotionType.Fly)
                {
                    StartVerticalMove();
                }
                
                _horizontalMoveDisposable.Clear();

                if (type == LocomotionType.Fall)
                {
                    StartHorizontalMove();
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _horizontalMoveDisposable.Clear();
    }

    private void Move()
    {
        VerticalMove();
        HorizontalMove();
    }

    private void StartVerticalMove()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => VerticalMove())
            .AddTo(_verticalMoveDisposable);
    }

    private void StartHorizontalMove()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => Move())
            .AddTo(_horizontalMoveDisposable);
    }

    private void VerticalMove()
    {
        if (LocomotionComposition.MoveVelocity.y > _verticalSpeed)
        {
            LocomotionComposition.MoveVelocity.y += _verticalSpeed * 1.75f * Time.deltaTime;

            if (LocomotionComposition.MoveVelocity.y < _verticalSpeed)
            {
                LocomotionComposition.MoveVelocity.y = _verticalSpeed;
            }
        }
    }

    private void HorizontalMove()
    {
        if (Input.Direction != Vector3.zero)
        {
            Vector3 velocity = HorizontalMoveSpeed * Input.Direction.normalized;
            CharacterController.Move(velocity * Time.deltaTime);

            RotateTowardsMove();
        }
    }
}
