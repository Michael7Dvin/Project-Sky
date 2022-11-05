using UniRx;
using UnityEngine;

[RequireComponent(typeof(InteractableButton), typeof(Animator))]
public class InteractableButtonAnimator : MonoBehaviour
{
    private readonly int _statusBoolParameterHash = Animator.StringToHash("Output");

    private InteractableButton _button;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _button = GetComponent<InteractableButton>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _button
            .Output
            .Subscribe(status =>
            {
                if (status == true)
                {
                    _animator.SetBool(_statusBoolParameterHash, true);
                }
                else
                {
                    _animator.SetBool(_statusBoolParameterHash, false);
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
}
