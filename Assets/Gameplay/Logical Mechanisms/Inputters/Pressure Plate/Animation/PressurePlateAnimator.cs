using UniRx;
using UnityEngine;

[RequireComponent(typeof(Detector), typeof(Animator))]
public class PressurePlateAnimator : MonoBehaviour
{
    private readonly int _statusBoolParameterHash = Animator.StringToHash("Output");

    private Detector _pressurePlate;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _pressurePlate = GetComponent<Detector>();
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
