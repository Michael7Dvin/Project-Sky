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
            .IsActivated
            .Subscribe(value =>
            {
                if (value == true)
                {
                    _animator.SetBool(_isActivatedBoolParameterHash, true);
                }
                else
                {
                    _animator.SetBool(_isActivatedBoolParameterHash, false);
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();
}
