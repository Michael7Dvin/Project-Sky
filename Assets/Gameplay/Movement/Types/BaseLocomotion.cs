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
    protected Transform Transform => LocomotionComposition.transform;
    protected IReadOnlyReactiveProperty<LocomotionType> CurrentLocomotionType => LocomotionComposition.CurrentLocomotionType;

    protected LocomotionInput Input => LocomotionComposition.Input;
    protected CharacterController CharacterController => LocomotionComposition.CharacterController;
    protected GroundDetector GroundDetector => LocomotionComposition.GroundDetector;

    public virtual void Initialize(LocomotionComposition locomotionComposition)
    {
        LocomotionComposition = locomotionComposition;
    }

    public virtual void Disable() => _disposable.Clear();

    protected void RotateTowardsMove()
    {
        if(Input.Direction.x != 0 && Input.Direction.z != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(new Vector3(Input.Direction.x, 0f, Input.Direction.z));
            Transform.rotation = Quaternion.RotateTowards(Transform.rotation, rotation, TOWARDS_MOVE_ROTATION_SPEED * Time.deltaTime);
        }
    }
}
