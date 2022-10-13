using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator), typeof(LocomotionComposition))]
public abstract class LocomotionAnimator : MonoBehaviour
{
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    
    protected Animator Animator { get; private set; }
    protected LocomotionComposition LocomotionComposition { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        LocomotionComposition = GetComponent<LocomotionComposition>();
    }

    private void OnEnable()
    {
        LocomotionComposition
            .CurrentLocomotionMoveSpeedType
            .Subscribe(type => OnLocomotionMoveSpeedTypeChanged(type))
            .AddTo(_disposable);

        LocomotionComposition
            .CurrentLocomotionType
            .Subscribe(type => OnLocomotionTypeChanged(type))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Dispose();

    private void Update()
    {
        SetLocomotionHorizontalVelocity();
        SetLocomotionVerticalVelocity();
    }

    protected abstract void OnLocomotionMoveSpeedTypeChanged(LocomotionMoveSpeedType type);
    protected abstract void OnLocomotionTypeChanged(LocomotionType type);

    private void SetLocomotionHorizontalVelocity()
    {
        if (float.IsNaN(LocomotionComposition.CharacterController.velocity.magnitude / LocomotionComposition.CurrentHorizontalMoveSpeed))
        {
            float velocity = 0f;
            Animator.SetFloat("LocomotionHorizontalVelocity", velocity, 0.3f, Time.deltaTime);
        }
        else
        {
            float velocity = Mathf.Clamp01(LocomotionComposition.CharacterController.velocity.magnitude / LocomotionComposition.CurrentHorizontalMoveSpeed); 
            Animator.SetFloat("LocomotionHorizontalVelocity", velocity, 0.3f, Time.deltaTime);
        }
    }

    private void SetLocomotionVerticalVelocity()
    {
        float velocity = LocomotionComposition.CharacterController.velocity.y;
        Animator.SetFloat("LocomotionVerticalVelocity", velocity, 0.3f, Time.deltaTime);
    }
}
