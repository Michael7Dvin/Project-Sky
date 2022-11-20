using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public abstract class BaseAreaDamager : MonoBehaviour
{
    protected readonly List<Health> _damagingObjects = new List<Health>();
    protected readonly CompositeDisposable _disposable = new CompositeDisposable();

    [SerializeField] private bool _isInitiallyActivated;

    [SerializeField] private Collider _damagingAreaTrigger;

    private readonly ReactiveProperty<bool> _isActivated = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsActivated => _isActivated;

    private void Awake()
    {
        if (_isInitiallyActivated == true)
        {
            Activate();
        }
    }

    private void OnValidate()
    {
        if (_damagingAreaTrigger == null)
        {
            Debug.LogError($"{gameObject} Damaged Objects Trigger cannot be null");
        }
        else
        {
            _damagingAreaTrigger.isTrigger = true;
        }
    }
    
    protected virtual void OnEnable()
    {
        _damagingAreaTrigger
            .OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.TryGetComponent(out Health health))
                {
                    _damagingObjects.Add(health);
                }
            })
            .AddTo(_disposable);

        _damagingAreaTrigger
            .OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (collider.TryGetComponent(out Health health))
                {
                    _damagingObjects.Remove(health);
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();

    public void Activate() => _isActivated.Value = true;
    public void Deactivate() => _isActivated.Value = false;
}
