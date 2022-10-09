using UnityEngine;
using UniRx;

public class DefaultJumpLocomotion : BaseJumpLocomotion
{
    private float _jumpSpeed;
    private float _normalHorizontalSpeed;
    private float _sprintHorizontalSpeed;

    private float _currentHorizontalSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();

    public DefaultJumpLocomotion(float jumpSpeed, float normalHorizontalSpeed, float sprintHorizontalSpeed)
    {
        _jumpSpeed = jumpSpeed;
        _normalHorizontalSpeed = normalHorizontalSpeed;
        _sprintHorizontalSpeed = sprintHorizontalSpeed;
    }

    public override float VerticalMoveSpeed => _jumpSpeed;
    public override float HorizontalMoveSpeed => _currentHorizontalSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        LocomotionComposition
            .CurrentLocomotionType
            .Subscribe(type =>
            {
                _horizontalMoveDisposable.Clear();

                if (type == LocomotionType.Jump)
                {
                    if(LocomotionComposition.GroundDetector.IsGrounded.Value == true)
                    {
                        Jump();
                    }

                    StartHorizontalMovement();
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _horizontalMoveDisposable.Clear();
    }

    protected void Jump() => LocomotionComposition.MoveVelocity.y = _jumpSpeed;

    private void StartHorizontalMovement()
    {
        if (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value == LocomotionMoveSpeedType.Sprint)
        {
            _currentHorizontalSpeed = _sprintHorizontalSpeed;
        }
        else
        {
            _currentHorizontalSpeed = _normalHorizontalSpeed;
        }

        Observable
            .EveryUpdate()
            .Subscribe(_ => HorizontalMove())
            .AddTo(_horizontalMoveDisposable);
    }

    private void HorizontalMove()
    {
        Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * new Vector3(Input.Direction.normalized.x, 0f, Input.Direction.normalized.z);
        CharacterController.Move(velocity * Time.deltaTime);

        RotateTowardsMove();
    }
}
