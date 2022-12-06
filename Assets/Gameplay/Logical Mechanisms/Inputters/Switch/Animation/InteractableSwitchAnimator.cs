using UniRx;
using UnityEngine;

[RequireComponent(typeof(InteractableSwitch), typeof(Animator))]
public class InteractableSwitchAnimator : MonoBehaviour
{
    private readonly int _outputBoolParameterHash = Animator.StringToHash("Output");

    private InteractableSwitch _switch;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _switch = GetComponent<InteractableSwitch>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _switch
            .ReadOnlyOutput
            .Subscribe(status =>
            {
                if (status == true)
                {
                    _animator.SetBool(_outputBoolParameterHash, true);
                }
                else
                {
                    _animator.SetBool(_outputBoolParameterHash, false);
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
}
