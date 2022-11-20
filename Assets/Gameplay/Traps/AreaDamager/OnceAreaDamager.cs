using UnityEngine;
using UniRx;

public class OnceAreaDamager : BaseAreaDamager
{
    [Range(0f, float.MaxValue)]
    [SerializeField] private float _damage;

    protected override void OnEnable()
    {
        base.OnEnable();

        IsActivated
            .Where(value => value == true)
            .Subscribe(value =>
            {
                foreach (Health health in _damagingObjects)
                {
                    health.TakeDamage(_damage);
                }
            })
            .AddTo(_disposable);
    }
}
