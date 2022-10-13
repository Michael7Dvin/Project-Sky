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

        LocomotionComposition
            .CurrentLocomotionType
            .Where(type => type == LocomotionType.Dodge)
            .Subscribe(type => Roll())
            .AddTo(_disposable);            
    }

    private void Roll()
    {
        Vector3 velocity;

        if (Input.Direction == Vector3.zero)
        {
            velocity = Transform.forward * HorizontalMoveSpeed;            
        }
        else
        {
            velocity = Input.Direction.normalized * HorizontalMoveSpeed;
        }

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                MoveHorizontal();
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

        void MoveHorizontal()
        {                       
            CharacterController.Move(velocity * Time.deltaTime);
        }
        void RotateTowardsRoll()
        {
            float rotationSpeed = 400f;

            Quaternion rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
