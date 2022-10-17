using System;
using UnityEngine;
using UniRx;

public class RollDodgeLocomotion : BaseDodgeLocomotion
{
    private readonly float _rollSpeed;
    private readonly CompositeDisposable _rollDisposable = new CompositeDisposable();

    public RollDodgeLocomotion(float rollSpeed, float rollDuration) : base(rollDuration)
    {
        _rollSpeed = rollSpeed;
    }

    public override float VerticalMoveSpeed => 0f;
    public override float HorizontalMoveSpeed => _rollSpeed;

    public override void Initialize(LocomotionComposition locomotionComposition)
    {
        base.Initialize(locomotionComposition);

        CurrentLocomotionType
            .Where(type => type == LocomotionType.Dodge)
            .Subscribe(type => Roll())
            .AddTo(_disposable);            
    }

    private void Roll()
    {
        Vector3 moveVelocity;

        if (InputDirection == Vector3.zero)
        {
            moveVelocity = Transform.forward * HorizontalMoveSpeed;            
        }
        else
        {
            moveVelocity = InputDirection.normalized * HorizontalMoveSpeed;
        }

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                MoveHorizontally();
                RotateTowardsRoll();
            })
            .AddTo(_rollDisposable);

        Observable
            .Interval(TimeSpan.FromSeconds(_rollDuration))
            .Subscribe(_ =>
            {
                RollCompleted.Execute();
                _rollDisposable.Clear();
            })
            .AddTo(_rollDisposable);

        void MoveHorizontally()
        {                       
            CharacterController.Move(moveVelocity * Time.deltaTime);
        }
        void RotateTowardsRoll()
        {
            float rotationSpeed = 400f;

            Quaternion rotation = Quaternion.LookRotation(new Vector3(moveVelocity.x, 0f, moveVelocity.z));
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
