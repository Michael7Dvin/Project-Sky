using System;
using UniRx;
using UnityEngine;

public class DefaultGroundLocomotion : GroundLocomotion
{
    private Vector3 _velocity;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private readonly CompositeDisposable _everyUpdateDisposable = new CompositeDisposable();

    public DefaultGroundLocomotion(float runSpeed, float sprintSpeed, float sneakSpeed, float rotationSpeed) : base(runSpeed, sprintSpeed, sneakSpeed, rotationSpeed)
    {
    }

    public override Vector3 Velocity => _velocity;

    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed
    {
        get
        {
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.HorizontalInputMagnitude * JogSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.HorizontalInputMagnitude * SprintSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.HorizontalInputMagnitude * SneakSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        LocomotionComposition
           .CurrentLocomotionType
           .Subscribe(type =>
           {
               _everyUpdateDisposable.Clear();

               if (type == LocomotionType.Ground)
               {
                   Observable
                   .EveryUpdate()
                   .Subscribe(_ => Move())
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

    protected override void Move()
    {
        if (Input.Direction != Vector3.zero)
        {
            if (CharacterController.isGrounded == true)
            {
                Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * Input.Direction.normalized;
                CharacterController.Move(velocity * Time.deltaTime);

                Quaternion rotation = Quaternion.LookRotation(Input.Direction);
                Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, RotationSpeed * Time.deltaTime);
            }
        }

        _velocity = CharacterController.velocity;
    }
}
