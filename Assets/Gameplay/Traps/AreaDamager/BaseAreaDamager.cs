using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public abstract class BaseAreaDamager : MonoBehaviour
{
    protected readonly List<Health> _damagedObjects = new List<Health>();
    
    [SerializeField] private Collider _damagedAreaTrigger;

    [Range(0f, float.MaxValue)]
    [SerializeField] private float _damage;

    [SerializeField] private bool _isActivatedInitially;

    private readonly ReactiveCommand _activated = new ReactiveCommand();
    private readonly ReactiveCommand _deactivated = new ReactiveCommand();
    private readonly CompositeDisposable _disposable = new CompositeDisposable();

    public IObservable<Unit> Activated => _activated;
    public IObservable<Unit> Deactivated => _deactivated;
    protected float Damage => _damage;

    private void OnValidate()
    {
        if (_damagedAreaTrigger == null)
        {
            Debug.LogError($"{gameObject} Damaged Objects Trigger cannot be null");
        }
        else
        {
            _damagedAreaTrigger.isTrigger = true;
        }
    }

    private void Awake()
    {
        if (_isActivatedInitially == true)
        {
            Activate();
        }
    }

    private void OnEnable()
    {
        _damagedAreaTrigger
            .OnTriggerEnterAsObservable()
            .Subscribe(collider =>
            {
                if (collider.TryGetComponent(out Health health))
                {
                    _damagedObjects.Add(health);
                }
            })
            .AddTo(_disposable);

        _damagedAreaTrigger
            .OnTriggerExitAsObservable()
            .Subscribe(collider =>
            {
                if (collider.TryGetComponent(out Health health))
                {
                    _damagedObjects.Remove(health);
                }
            })
            .AddTo(_disposable);
    }

    private void OnDisable() => _disposable.Clear();

    public virtual void Activate() => _activated.Execute();
    public virtual void Deactivate() => _deactivated.Execute();
}
