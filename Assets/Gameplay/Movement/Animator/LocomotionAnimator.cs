using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator), typeof(LocomotionComposition))]
public class LocomotionAnimator : MonoBehaviour
{
    private Animator _animator;
    private LocomotionComposition _locomotionComposition;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _locomotionComposition = GetComponent<LocomotionComposition>();
    }

    private void OnEnable()
    {
                _locomotionComposition
            .CurrentLocomotionMoveSpeedType
            .Subscribe(state =>
            {
                switch (state)
                {
                    case LocomotionMoveSpeedType.Normal:
                        _animator.SetBool("LocomotionIsSprinting", false);
                        _animator.SetBool("LocomotionIsSlow", false);
                        break;
                    case LocomotionMoveSpeedType.Sprint:
                        _animator.SetBool("LocomotionIsSprinting", true);
                        _animator.SetBool("LocomotionIsSlow", false);
                        break;
                    case LocomotionMoveSpeedType.Slow:
                        _animator.SetBool("LocomotionIsSlow", true);
                        _animator.SetBool("LocomotionIsSprinting", false);
                        break;
                }

            }).AddTo(_disposable);

        _locomotionComposition
            .CurrentLocomotionType
            .Subscribe(type =>
            {
                switch (type)
                {
                    case LocomotionType.Ground:
                        _animator.SetBool("LocomotionIsFalling", false);
                        break;
                    case LocomotionType.Fall:
                        _animator.SetBool("LocomotionIsFalling", true);
                        break;
                    case LocomotionType.Jump:
                        _animator.SetBool("LocomotionIsFalling", true);
                        break;
                }

            }).AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Dispose();

    private void Update()
    {
        SetLocomotionHorizontalVelocity();
        SetLocomotionVerticalVelocity();
    }

    private void SetLocomotionHorizontalVelocity()
    {
        if (float.IsNaN(_locomotionComposition.CharacterVelocity.magnitude / _locomotionComposition.CurrentHorizontalMoveSpeed))
        {
            float velocity = 0f;
            _animator.SetFloat("LocomotionHorizontalVelocity", velocity, 0.3f, Time.deltaTime);
        }
        else
        {
            float velocity = Mathf.Clamp01(_locomotionComposition.CharacterVelocity.magnitude / _locomotionComposition.CurrentHorizontalMoveSpeed); 
            _animator.SetFloat("LocomotionHorizontalVelocity", velocity, 0.3f, Time.deltaTime);
        }
    }

    private void SetLocomotionVerticalVelocity()
    {

    }
}
