using System;
using UniRx;
using UnityEngine;

public class DefaultGroundLocomotion : BaseGroundLocomotion
{
    private float _jogSpeed;
    private float _sprintSpeed;
    private float _sneakSpeed;
    
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
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
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

        LocomotionComposition
           .CurrentLocomotionType
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
    }

    private void Move()
    {
        if (Input.Direction != Vector3.zero)
        {
            if (LocomotionComposition.GroundDetector.IsGrounded.Value == true)
            {
                Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * Input.Direction.normalized;
                CharacterController.Move(velocity * Time.deltaTime);

                RotateTowardsMove();
            }
        }
    }
}
