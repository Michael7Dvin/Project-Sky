using System;
using UniRx;
using UnityEngine;

public class DefaultGroundLocomotion : BaseGroundLocomotion
{
    private float _jogSpeed;
    private float _sprintSpeed;
    private float _sneakSpeed;
    private float _rotationSpeed;
    
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private readonly CompositeDisposable _moveDisposable = new CompositeDisposable();

    public DefaultGroundLocomotion(float jogSpeed, float sprintSpeed, float sneakSpeed, float rotationSpeed)
    {
        _jogSpeed = jogSpeed;
        _sprintSpeed = sprintSpeed;
        _sneakSpeed = sneakSpeed;
        _rotationSpeed = rotationSpeed;
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
        _disposable.Clear();
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
            if (CharacterController.isGrounded == true)
            {
                Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * Input.Direction.normalized;
                CharacterController.Move(velocity * Time.deltaTime);

                Quaternion rotation = Quaternion.LookRotation(Input.Direction);
                Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
