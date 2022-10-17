using UniRx;
using UnityEngine;

public abstract class BaseLocomotion
{
    private const float TOWARDS_MOVE_ROTATION_SPEED = 400f;
    protected readonly CompositeDisposable _disposable = new CompositeDisposable();

    public abstract LocomotionType Type { get; }

    public abstract float VerticalMoveSpeed { get; }
    public abstract float HorizontalMoveSpeed { get; }

    protected LocomotionComposition LocomotionComposition { get; private set; }

    protected IReadOnlyReactiveProperty<LocomotionType> CurrentLocomotionType => LocomotionComposition.CurrentLocomotionType;
    protected IReadOnlyReactiveProperty<LocomotionMoveSpeedType> CurrentLocomotionMoveSpeedType => LocomotionComposition.CurrentLocomotionMoveSpeedType;

    protected CharacterController CharacterController => LocomotionComposition.CharacterController;
    protected Vector3 InputDirection => LocomotionComposition.InputDirection;
    protected IReadOnlyReactiveProperty<bool> IsGrounded => LocomotionComposition.IsGrounded;
    protected Transform Transform => LocomotionComposition.transform;

    public virtual void Initialize(LocomotionComposition locomotionComposition)
    {
        LocomotionComposition = locomotionComposition;
    }

    public virtual void Disable() => _disposable.Clear();

    protected void RotateTowardsMove()
    {
        if(InputDirection.x != 0 && InputDirection.z != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(new Vector3(InputDirection.x, 0f, InputDirection.z));
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, TOWARDS_MOVE_ROTATION_SPEED * Time.deltaTime);
        }
    }
}
