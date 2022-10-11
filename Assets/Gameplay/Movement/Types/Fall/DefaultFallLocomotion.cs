using UnityEngine;
using UniRx;

public class DefaultFallLocomotion : BaseFallLocomotion
{
    private const float FREE_FALL_VERTICAL_MAX_SPEED = -50f;
    private const float CONSTANT_VERTICAL_SPEED = -9.8f;

    private float _currentVerticalSpeed = CONSTANT_VERTICAL_SPEED;

    private readonly float _horizontalSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _verticalMoveDisposable = new CompositeDisposable();

    public DefaultFallLocomotion(float horizontalSpeed) 
    {
        _horizontalSpeed = horizontalSpeed;
    }

    public override float VerticalMoveSpeed => CONSTANT_VERTICAL_SPEED;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * _horizontalSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);


        LocomotionComposition
            .CurrentLocomotionType
            .Subscribe(type =>
            {
                _verticalMoveDisposable.Clear();
                _horizontalMoveDisposable.Clear();

                if (type == LocomotionType.Fall)
                {
                    StartFreeFallVerticalMove();
                    StartHorizontalMove();
                    return;
                }

                if (type == LocomotionType.Fly)
                {
                    return;
                }

                StartConstantVerticalMove();      
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _verticalMoveDisposable.Clear();
        _horizontalMoveDisposable.Clear();
    }

    private void StartConstantVerticalMove()
    {
        if(LocomotionComposition.MoveVelocity.y < CONSTANT_VERTICAL_SPEED)
        {
            LocomotionComposition.MoveVelocity.y = CONSTANT_VERTICAL_SPEED;
        }

        Observable
            .EveryUpdate()
            .Subscribe(_ => ConstantVerticalMove())
            .AddTo(_verticalMoveDisposable);

        void ConstantVerticalMove()
        {
            if (LocomotionComposition.MoveVelocity.y > CONSTANT_VERTICAL_SPEED)
            {
                if(LocomotionComposition.MoveVelocity.y + CONSTANT_VERTICAL_SPEED * 1.7f * Time.deltaTime < CONSTANT_VERTICAL_SPEED)
                {
                    LocomotionComposition.MoveVelocity.y = CONSTANT_VERTICAL_SPEED;
                    return;
                }

                LocomotionComposition.MoveVelocity.y += CONSTANT_VERTICAL_SPEED * 1.7f * Time.deltaTime;
            }
        }
    }
    private void StartFreeFallVerticalMove()
    {
        LocomotionComposition.MoveVelocity.y = CONSTANT_VERTICAL_SPEED;

        Observable
            .EveryUpdate()
            .Subscribe(_ => FreeFallVerticalMove())
            .AddTo(_verticalMoveDisposable);

        void FreeFallVerticalMove()
        {
            if (LocomotionComposition.MoveVelocity.y > FREE_FALL_VERTICAL_MAX_SPEED)
            {
                if (LocomotionComposition.MoveVelocity.y + CONSTANT_VERTICAL_SPEED / 2f * Time.deltaTime < FREE_FALL_VERTICAL_MAX_SPEED)
                {
                    LocomotionComposition.MoveVelocity.y = FREE_FALL_VERTICAL_MAX_SPEED;
                    return;
                }

                LocomotionComposition.MoveVelocity.y += CONSTANT_VERTICAL_SPEED / 2f * Time.deltaTime;
            }
        }
    }

    private void StartHorizontalMove()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => HorizontalMove())
            .AddTo(_verticalMoveDisposable);

        void HorizontalMove()
        {
            if (Input.Direction != Vector3.zero)
            {
                Vector3 velocity = HorizontalMoveSpeed * Input.Direction.normalized;
                CharacterController.Move(velocity * Time.deltaTime);

                RotateTowardsMove();
            }
        }
    }
}
