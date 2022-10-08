using UnityEngine;
using System;
using UniRx;

public class GlideFlyLocomotion : BaseFlyLocomotion
{
    private readonly float _normalGlideVerticalSpeed;
    private readonly float _normalGlideHorizontalSpeed;

    private readonly float _fastGlideVerticalSpeed;
    private readonly float _fastGlideHorizontalSpeed;

    private readonly float _rotationSpeed;

    private readonly CompositeDisposable _moveDisposable = new CompositeDisposable();

    public GlideFlyLocomotion(float normalGlideVerticalSpeed, float fastGlideVerticalSpeed, float normalGlideHorizontalSpeed, float fastGlideHorizontalSpeed, float rotationSpeed)
    {
        _normalGlideVerticalSpeed = normalGlideVerticalSpeed;
        _normalGlideHorizontalSpeed = normalGlideHorizontalSpeed;

        _fastGlideVerticalSpeed = fastGlideVerticalSpeed;
        _fastGlideHorizontalSpeed = fastGlideHorizontalSpeed;

        _rotationSpeed = rotationSpeed;
    }
    
    public override float VerticalMoveSpeed
    {
        get
        {
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return _normalGlideVerticalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return _fastGlideVerticalSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return _normalGlideVerticalSpeed;
            }

            Debug.LogException(new ArgumentException());
            return 0f;
        }
    }
    public override float HorizontalMoveSpeed
    {
        get
        {
            switch (LocomotionComposition.CurrentLocomotionMoveSpeedType.Value)
            {
                case LocomotionMoveSpeedType.Normal:
                    return LocomotionComposition.HorizontalInputMagnitude * _normalGlideHorizontalSpeed;
                case LocomotionMoveSpeedType.Sprint:
                    return LocomotionComposition.HorizontalInputMagnitude * _fastGlideHorizontalSpeed;
                case LocomotionMoveSpeedType.Slow:
                    return LocomotionComposition.HorizontalInputMagnitude * _normalGlideHorizontalSpeed;
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

                if(type == LocomotionType.Fly)
                {
                    Observable
                    .EveryUpdate()
                    .Subscribe(_ => Move())                    
                    .AddTo(_moveDisposable);
                }
            })
            .AddTo(_disposable);
    }

    public override void Disable()
    {
        _disposable.Clear();
        _moveDisposable.Clear();
    }

    private void Move()
    {

        Vector3 velocity = (VerticalMoveSpeed * Vector3.up) + (HorizontalMoveSpeed * Input.Direction.normalized);
        CharacterController.Move(velocity * Time.deltaTime);

        Quaternion rotation = Quaternion.LookRotation(Input.Direction);
        Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
}

