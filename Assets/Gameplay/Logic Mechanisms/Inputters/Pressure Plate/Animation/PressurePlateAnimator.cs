using UniRx;
using UnityEngine;

[RequireComponent(typeof(PressurePlate), typeof(Animator))]
public class PressurePlateAnimator : MonoBehaviour
{
    private readonly int _statusBoolParameterHash = Animator.StringToHash("Output");

    private PressurePlate _pressurePlate;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _pressurePlate = GetComponent<PressurePlate>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _pressurePlate
            .ReadOnlyOutput
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
