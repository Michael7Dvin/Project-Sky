using UnityEngine;
using UniRx;

public class DefaultFallLocomotion : BaseFallLocomotion
{
    private const float GRAVITY_ACCELERATION = -9.8f;

    private const float GROUNDED_MAX_VERTICAL_SPEED = -5f;
    private const float GROUNDED_VERTICAL_SPEED_MULTIPLIER = 1.7f;

    private const float NOT_GROUNDED_MAX_VERTICAL_SPEED = -12f;
    private const float NOT_GROUNDED_VERTICAL_SPEED_MULTIPLIER = 1.7f;

    private const float FALL_MAX_VERTICAL_SPEED = -50f;
    private const float FALL_VERTICAL_SPEED_MULTIPLIER = 0.5f;

    private readonly float _horizontalSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _verticalMoveDisposable = new CompositeDisposable();

    public DefaultFallLocomotion(float horizontalSpeed) 
    {
        _horizontalSpeed = horizontalSpeed;
    }

    public override float VerticalMoveSpeed => NOT_GROUNDED_MAX_VERTICAL_SPEED;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * _horizontalSpeed;

    private float MaxVerticalSpeed
    {
        get
        {
            if (CurrentLocomotionType.Value == LocomotionType.Fall)
            {
                return FALL_MAX_VERTICAL_SPEED;
            }

            if(IsGrounded.Value == true)
            {
                return GROUNDED_MAX_VERTICAL_SPEED;
            }
            else
            {
                return NOT_GROUNDED_MAX_VERTICAL_SPEED;
            }
        }
    }
    private float VerticalAccelerationMultiplier
    {
        get
        {
            if (CurrentLocomotionType.Value == LocomotionType.Fall)
            {
                return FALL_VERTICAL_SPEED_MULTIPLIER;
            }

            if (IsGrounded.Value == true)
            {
                return GROUNDED_VERTICAL_SPEED_MULTIPLIER;
            }
            else
            {
                return NOT_GROUNDED_VERTICAL_SPEED_MULTIPLIER;
            }
        }
    }

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        CurrentLocomotionType
            .Subscribe(type =>
            {
                _verticalMoveDisposable.Clear();
                _horizontalMoveDisposable.Clear();

                if (type == LocomotionType.Fall)
                {
                    StartVerticalMovement();
                    StartHorizontalMovement();
                    return;
                }

                if (type == LocomotionType.Fly)
                {                 
                    return;
                }

                StartVerticalMovement();      
            })
            .AddTo(_disposable);

        IsGrounded
            .Where(isGrounded => isGrounded == true && LocomotionComposition.MoveVelocity.y < GROUNDED_MAX_VERTICAL_SPEED)
            .Subscribe(_ => LocomotionComposition.MoveVelocity.y = GROUNDED_MAX_VERTICAL_SPEED)
            .AddTo(_disposable);

        void StartVerticalMovement()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => MoveVertically())
                .AddTo(_verticalMoveDisposable);
        }
        void StartHorizontalMovement()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => MoveHorizontally())
                .AddTo(_verticalMoveDisposable);
        }
    }

    public override void Disable()
    {
        base.Disable();
        _verticalMoveDisposable.Clear();
        _horizontalMoveDisposable.Clear();
    }

    private void MoveVertically()
    {
        if (LocomotionComposition.MoveVelocity.y > MaxVerticalSpeed)
        {
            if ((LocomotionComposition.MoveVelocity.y + GRAVITY_ACCELERATION * VerticalAccelerationMultiplier * Time.deltaTime) < MaxVerticalSpeed)
            {
                LocomotionComposition.MoveVelocity.y = MaxVerticalSpeed;
                return;
            }

            LocomotionComposition.MoveVelocity.y += GRAVITY_ACCELERATION * VerticalAccelerationMultiplier * Time.deltaTime;
        }
    }

    private void MoveHorizontally()
    {
        if (InputDirection != Vector3.zero)
        {
            Vector3 velocity = HorizontalMoveSpeed * InputDirection.normalized;
            CharacterController.Move(velocity * Time.deltaTime);

            RotateTowardsMove();
        }
    }
}

