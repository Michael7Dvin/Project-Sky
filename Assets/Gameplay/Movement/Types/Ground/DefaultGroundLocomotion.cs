using System;
using UniRx;
using UnityEngine;

public class DefaultGroundLocomotion : BaseGroundLocomotion
{
    private readonly float _jogSpeed;
    private readonly float _sprintSpeed;
    private readonly float _sneakSpeed;
    
    private readonly CompositeDisposable _moveDisposable = new CompositeDisposable();

    public DefaultGroundLocomotion(float jogSpeed, float sprintSpeed, float sneakSpeed)
    {
        _jogSpeed = jogSpeed;
        _sprintSpeed = sprintSpeed;
        _sneakSpeed = sneakSpeed;
    }

    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed
    {
        get
        {
            switch (CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.HorizontalInputMagnitude * _jogSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.HorizontalInputMagnitude * _sprintSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.HorizontalInputMagnitude * _sneakSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        CurrentLocomotionType
           .Subscribe(type =>
           {
               _moveDisposable.Clear();

               if (type == LocomotionType.Ground)
               {
                   StartMovement();
               }
           })
           .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _moveDisposable.Clear();
    }

    private void StartMovement()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => Move())
            .AddTo(_moveDisposable);

        void Move()
        {
            if (InputDirection != Vector3.zero)
            {
                Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * InputDirection.normalized;
                CharacterController.Move(velocity * Time.deltaTime);
                RotateTowardsMove();
            }
        }
    }
}
