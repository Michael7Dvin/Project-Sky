using UnityEngine;
using UniRx;

public class AreaDamagerMechanizedReactivator : BaseAreaDamagerReactivator
{
    [SerializeField] private LogicalMechanism _activatingInput;
    [SerializeField] private LogicalMechanism _deactivatingInput;

    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    protected override void Awake()
    {
        base.Awake();
        _areaDamager.Activate();
    }

    private void OnEnable()
    {
        if (_activatingInput is not null)
        {
            _activatingInput
                .Output
                .Where(value => value == true)
                .Subscribe(value => _areaDamager.Activate())
                .AddTo(_disposable);
        }

        if (_deactivatingInput is not null)
        {
            _deactivatingInput
                .Output
                .Where(value => value == false)
                .Subscribe(value => _areaDamager.Deactivate())
                .AddTo(_disposable);
        }
    }

    private void OnDisable() => _disposable.Clear();
}
