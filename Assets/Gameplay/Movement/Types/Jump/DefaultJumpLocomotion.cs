using UnityEngine;
using UniRx;

public class DefaultJumpLocomotion : BaseJumpLocomotion
{
    private float _jumpSpeed;
    private float _horizontalSpeed;

    private CompositeDisposable _disposable = new CompositeDisposable();
    private CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();

    public DefaultJumpLocomotion(float jumpSpeed, float horizontalSpeed)
    {
        _jumpSpeed = jumpSpeed;
        _horizontalSpeed = horizontalSpeed;
    }

    public override float VerticalMoveSpeed => _jumpSpeed;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * _horizontalSpeed;

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
                    Jump();
                    StartHorizontalMovement();
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        _disposable.Clear();
        _horizontalMoveDisposable.Clear();
    }

    private void Jump() => LocomotionComposition.MoveVelocity.y = _jumpSpeed;

    private void StartHorizontalMovement()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => HorizontalMove())
            .AddTo(_horizontalMoveDisposable);
    }

    private void HorizontalMove()
    {
        Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * Input.Direction.normalized;
        CharacterController.Move(velocity * Time.deltaTime);

        RotateTowardsMove();
    }
}
