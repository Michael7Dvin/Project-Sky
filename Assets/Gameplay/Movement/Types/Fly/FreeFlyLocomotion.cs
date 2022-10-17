using System;
using UnityEngine;
using UniRx;

public class FreeFlyLocomotion : BaseFlyLocomotion
{
    private readonly float _normalHorizontalSpeed;
    private readonly float _fastHorizontalSpeed;   
    
    private readonly float _normalVerticalSpeed;
    private readonly float _fastVerticalSpeed;

    private readonly CompositeDisposable _flyDisposable = new CompositeDisposable();

    public FreeFlyLocomotion(float normalHorizontalSpeed, float sprintHorizontalSpeed, float normalVerticalSpeed, float sprintVerticalSpeed)
    {
        _normalHorizontalSpeed = normalHorizontalSpeed;
        _fastHorizontalSpeed = sprintHorizontalSpeed;
        _normalVerticalSpeed = normalVerticalSpeed;
        _fastVerticalSpeed = sprintVerticalSpeed;
    }

    public override float VerticalMoveSpeed
    {
        get
        {
            switch (CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.VerticalInputMagnitude * _normalVerticalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.VerticalInputMagnitude * _fastVerticalSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.VerticalInputMagnitude * _normalVerticalSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }
    public override float HorizontalMoveSpeed
    {
        get
        {
            switch (CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.HorizontalInputMagnitude * _normalHorizontalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.HorizontalInputMagnitude * _fastHorizontalSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.HorizontalInputMagnitude * _normalHorizontalSpeed;
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
                _flyDisposable.Clear();

                if (type == LocomotionType.Fly)
                {
                    StartFly();
                }
            })
            .AddTo(_disposable);

        void StartFly()
        {
            LocomotionComposition.MoveVelocity.y = 0f;

            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    MoveVertically();
                    MoveHorizontally();
                })
                .AddTo(_flyDisposable);
        }
    }

    public override void Disable()
    {
        base.Disable();
    }

    private void MoveVertically()
    {
        if (InputDirection.y != 0f)
        {
            Vector3 velocity = LocomotionComposition.VerticalInputMagnitude * VerticalMoveSpeed * new Vector3(0f, InputDirection.normalized.y, 0f);
            CharacterController.Move(velocity * Time.deltaTime);
        }
    }

    private void MoveHorizontally()
    {
        if (InputDirection.x != 0f || InputDirection.z != 0f)
        {
            Vector3 velocity = LocomotionComposition.HorizontalInputMagnitude * HorizontalMoveSpeed * InputDirection.normalized;
            CharacterController.Move(velocity * Time.deltaTime);

            RotateTowardsMove();
        }
    }
}
