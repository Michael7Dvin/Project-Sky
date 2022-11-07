using UniRx;
using UnityEngine;

[RequireComponent(typeof(BaseAreaDamager), typeof(Animator))]
public class FloorSpikesAnimator : MonoBehaviour
{
    private readonly int _isActivatedBoolParameterHash = Animator.StringToHash("IsActivated");

    private BaseAreaDamager _spikes;
    private Animator _animator;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    private void Awake()
    {
        _spikes = GetComponent<BaseAreaDamager>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _spikes
            .Activated
            .Subscribe(_ => _animator.SetBool(_isActivatedBoolParameterHash, true))
            .AddTo(_disposable);

        _spikes
            .Deactivated
            .Subscribe(_ => _animator.SetBool(_isActivatedBoolParameterHash, false))
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
}
