using UniRx;
using UnityEngine;

public abstract class BaseLocomotion
{
    private const float TOWARDS_MOVE_ROTATION_SPEED = 400f;
    protected readonly CompositeDisposable _disposable = new CompositeDisposable();

    public abstract LocomotionType Type { get; }

    public abstract float VerticalMoveSpeed { get; }
    public abstract float HorizontalMoveSpeed { get; }

    protected Transform Transform => LocomotionComposition.transform;

    protected LocomotionComposition LocomotionComposition { get; private set; }
    protected LocomotionInput Input => LocomotionComposition.Input;
    protected CharacterController CharacterController => LocomotionComposition.CharacterController;

    public virtual void Initialize(LocomotionComposition locomotionComposition)
    {
        LocomotionComposition = locomotionComposition;
    }
    public abstract void Disable();

    protected void RotateTowardsMove()
    {
        if(Input.Direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Input.Direction);
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, TOWARDS_MOVE_ROTATION_SPEED * Time.deltaTime);
        }
    }
}
