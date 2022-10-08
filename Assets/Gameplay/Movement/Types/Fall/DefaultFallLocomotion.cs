using UnityEngine;
using UniRx;

public class DefaultFallLocomotion : BaseFallLocomotion
{
    private readonly float _verticalSpeed;
    private readonly float _horizontalSpeed;
    private readonly float _rotationSpeed;

    private readonly CompositeDisposable _horizontalMoveDisposable = new CompositeDisposable();

    public DefaultFallLocomotion(float verticalSpeed, float horizontalSpeed, float rotationSpeed) 
    {
        _verticalSpeed = verticalSpeed;
        _horizontalSpeed = horizontalSpeed;
        _rotationSpeed = rotationSpeed;
    }

    public override float VerticalMoveSpeed => _verticalSpeed;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * _horizontalSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        Observable
            .EveryUpdate()
            .Subscribe(_ => VerticalMove())
            .AddTo(_disposable);

        LocomotionComposition
            .CurrentLocomotionType
            .Subscribe(type =>
            {
                _horizontalMoveDisposable.Clear();

                if (type == LocomotionType.Fall)
                {
                    Observable
                    .EveryUpdate()
                    .Subscribe(_ => Move())
                    .AddTo(_horizontalMoveDisposable);
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        _disposable.Clear();
        _horizontalMoveDisposable.Clear();
    }

    private void Move()
    {
        VerticalMove();
        HorizontalMove();
    }

    private void VerticalMove()
    {
        if(LocomotionComposition.MoveVelocity.y > _verticalSpeed)
        {
            LocomotionComposition.MoveVelocity.y += _verticalSpeed * 1.75f * Time.deltaTime;
            
            if(LocomotionComposition.MoveVelocity.y < _verticalSpeed)
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

            Quaternion rotation = Quaternion.LookRotation(Input.Direction);
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
