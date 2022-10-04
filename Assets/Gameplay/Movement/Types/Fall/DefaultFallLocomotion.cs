using UnityEngine;
using UniRx;

public class DefaultFallLocomotion : FallLocomotion
{
    private Vector3 _verticalVelocity;
    private Vector3 _horizontalVelocity;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private readonly CompositeDisposable _everyUpdateDisposable = new CompositeDisposable();

    public DefaultFallLocomotion(float verticalSpeed, float horizontalSpeed, float rotationSpeed) : base(verticalSpeed, horizontalSpeed, rotationSpeed)
    {
    }

    public override Vector3 Velocity => _verticalVelocity + _horizontalVelocity;

    public override float VerticalMoveSpeed => VerticalSpeed;
    public override float HorizontalMoveSpeed => LocomotionComposition.HorizontalInputMagnitude * HorizontalSpeed;

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
                _everyUpdateDisposable.Clear();

                if (type == LocomotionType.Fall)
                {
                    Observable
                    .EveryUpdate()
                    .Subscribe(_ => HorizontalMove())
                    .AddTo(_everyUpdateDisposable);
                }

            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        _disposable.Clear();
        _everyUpdateDisposable.Clear();

    }

    protected override void VerticalMove()
    {
        CharacterController.Move(Time.deltaTime * VerticalMoveSpeed * Vector3.up);
        _verticalVelocity = CharacterController.velocity;
    }

    protected override void HorizontalMove()
    {
        if (Input.Direction != Vector3.zero)
        {
            Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * Input.Direction.normalized;
            CharacterController.Move(velocity * Time.deltaTime);

            _horizontalVelocity = CharacterController.velocity;

            Quaternion rotation = Quaternion.LookRotation(Input.Direction);
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        }
    }
}
