using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator), typeof(GroundLocomotion))]
public class LocomotionAnimator : MonoBehaviour
{
    private Animator _animator;
    private GroundLocomotion _groundLocomotion;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _groundLocomotion = GetComponent<GroundLocomotion>();
    }

    private void OnEnable()
    {
        _groundLocomotion.State.Subscribe(state =>
        {
            switch (state)
            {
                case LocomotionState.Running:
                    _animator.SetBool("IsSprinting", false);
                    _animator.SetBool("IsSneaking", false);
                    break;
                case LocomotionState.Sprinting:
                    _animator.SetBool("IsSprinting", true);
                    _animator.SetBool("IsSneaking", false);
                    break;
                case LocomotionState.Sneaking:
                    _animator.SetBool("IsSneaking", true);
                    _animator.SetBool("IsSprinting", false);
                    break;
            }

        }).AddTo(_disposable);

    }

    private void OnDisable() => _disposable.Dispose();

    private void Update()
    {
        _animator.SetFloat("MovementVelocity", _groundLocomotion.VelocityMagnitudeFraction, 0.4f, Time.deltaTime);
    }
}
