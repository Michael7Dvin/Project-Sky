using UnityEngine;
using UniRx;

public class PeriodicAreaDamager : BaseAreaDamager
{
    [Range(0f, float.MaxValue)]
    [SerializeField] private float _damage;

    private readonly CompositeDisposable _damgingDisposable = new CompositeDisposable();

    protected override void OnEnable()
    {
        base.OnEnable();

        IsActivated
            .Subscribe(value =>
            {
                if (value == true)
                {
                    Observable
                        .EveryUpdate()
                        .Subscribe(_ =>
                        {
                            foreach (Health health in _damagingObjects)
                            {
                                health.TakeDamage(_damage * Time.deltaTime);
                            }
                        })
                        .AddTo(_damgingDisposable);
                }
                else
                {
                    _damgingDisposable.Clear();
                }
            })
            .AddTo(_disposable);
    }
}
