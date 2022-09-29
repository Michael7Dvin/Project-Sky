using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator), typeof(Movement))]
public class MovementAnimator : MonoBehaviour
{
    private Animator _animator;
    private Movement _movement;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        _movement.State.Subscribe(state =>
        {
            switch (state)
            {
                case MovementState.Running:
                    _animator.SetBool("IsSprinting", false);
                    _animator.SetBool("IsSneaking", false);
                    break;
                case MovementState.Sprinting:
                    _animator.SetBool("IsSprinting", true);
                    _animator.SetBool("IsSneaking", false);
                    break;
                case MovementState.Sneaking:
                    _animator.SetBool("IsSneaking", true);
                    _animator.SetBool("IsSprinting", false);
                    break;
            }

        }).AddTo(_disposable);

    }

    private void OnDisable() => _disposable.Dispose();

    private void Update()
    {
        _animator.SetFloat("MovementVelocity", _movement.MovementVelocityMagnitudeFraction, 0.4f, Time.deltaTime);
    }
}
