using UnityEngine;
using UniRx;

public class PeriodicAreaDamager : BaseAreaDamager
{
    private readonly CompositeDisposable _damgingDisposable = new CompositeDisposable();

    public override void Activate()
    {
        base.Activate();

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                foreach (Health health in _damagedObjects)
                {
                    health.TakeDamage(Damage * Time.deltaTime);
                }
            })
            .AddTo(_damgingDisposable);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _damgingDisposable.Clear();
    }
}
