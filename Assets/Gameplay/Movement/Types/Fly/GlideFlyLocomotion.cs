using System;
using UnityEngine;
using UniRx;

public class GlideFlyLocomotion : BaseFlyLocomotion
{
    private readonly float _normalHorizontalSpeed;
    private readonly float _fastHorizontalSpeed;

    private readonly float _normalVerticalSpeed;
    private readonly float _fastVerticalSpeed;

    private readonly CompositeDisposable _glideDisposable = new CompositeDisposable();

    public GlideFlyLocomotion(float normalGlideVerticalSpeed, float fastGlideVerticalSpeed, float normalGlideHorizontalSpeed, float fastGlideHorizontalSpeed)
    {
        _normalVerticalSpeed = normalGlideVerticalSpeed;
        _normalHorizontalSpeed = normalGlideHorizontalSpeed;

        _fastVerticalSpeed = fastGlideVerticalSpeed;
        _fastHorizontalSpeed = fastGlideHorizontalSpeed;
    }
    
    public override float VerticalMoveSpeed
    {
        get
        {
            switch (CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return _normalVerticalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return _fastVerticalSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return _normalVerticalSpeed;
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
                _glideDisposable.Clear();

                if(type == LocomotionType.Fly)
                {
                    StartGlide();
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        base.Disable();
        _glideDisposable.Clear();
    }

    private void StartGlide()
    {
        LocomotionComposition.MoveVelocity.y = 0f;

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                MoveVertically();
                MoveHorizontally();
            })
            .AddTo(_glideDisposable);
    }

    private void MoveVertically()
    {
        Vector3 velocity = Vector3.up * VerticalMoveSpeed;
        CharacterController.Move(velocity * Time.deltaTime);
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

