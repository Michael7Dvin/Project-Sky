using UnityEngine;
using UniRx;

public class MultiJumpLocomotion : BaseJumpLocomotion
{
    private float _jumpSpeed;
    private float _normalHorizontalSpeed;
    private float _sprintHorizontalSpeed;
    private readonly int _additionalJumps;

    private int _remainingJumps;
    private float _currentHorizontalSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _multiJumpDisposable = new CompositeDisposable();

    public MultiJumpLocomotion(float jumpSpeed, float normalHorizontalSpeed, float sprintHorizontalSpeed, int additionalJumps)
    {
        _jumpSpeed = jumpSpeed;
        _normalHorizontalSpeed = normalHorizontalSpeed;
        _sprintHorizontalSpeed = sprintHorizontalSpeed;

        _additionalJumps = additionalJumps;
        _remainingJumps = additionalJumps;
    }

    public override float VerticalMoveSpeed => _jumpSpeed;
    public override float HorizontalMoveSpeed => _currentHorizontalSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        CurrentLocomotionType
            .Subscribe(type =>
            {
                _horizontalMoveDisposable.Clear();
                _multiJumpDisposable.Clear();

                if (CoyoteTimeCounter > 0)
                {
                    _remainingJumps = _additionalJumps;

                    if (type == LocomotionType.Jump)
                    {
                        Jump();
                        StartHorizontalMovement();
                    }
                }
                else
                {
                    if (type == LocomotionType.Jump)
                    {
                        MultiJump();
                        StartHorizontalMovement();
                    }
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _horizontalMoveDisposable.Clear();
        _multiJumpDisposable.Clear();
    }

    private void Jump()
    {
        LocomotionComposition.MoveVelocity.y = _jumpSpeed;
    }

    private void MultiJump()
    {
        if (_remainingJumps > 0)
        {
            Jump();
            _remainingJumps--;
        }
    }

    private void StartHorizontalMovement()
    {
        if (CurrentLocomotionMoveSpeedType.Value == LocomotionMoveSpeedType.Sprint)
        {
            _currentHorizontalSpeed = _sprintHorizontalSpeed;
        }
        else
        {
            _currentHorizontalSpeed = _normalHorizontalSpeed;
        }

        Observable
            .EveryUpdate()
            .Subscribe(_ => MoveHorizontally())
            .AddTo(_horizontalMoveDisposable);

        void MoveHorizontally()
        {
            Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * new Vector3(InputDirection.normalized.x, 0f, InputDirection.normalized.z);
            CharacterController.Move(velocity * Time.deltaTime);
            RotateTowardsMove();
        }
    }   
}
