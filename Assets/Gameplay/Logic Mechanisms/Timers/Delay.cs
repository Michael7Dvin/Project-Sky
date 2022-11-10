using System;
using UnityEngine;
using UniRx;

public class Delay : LogicalMechanism
{
    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Delay(TimeSpan.FromSeconds(_delay))
                .Subscribe(value =>
                {
                    _output.Value = value;
                })
                .AddTo(Disposable);
        }        
    }

    private void Start()
    {
        CompositeDisposable disposable = new CompositeDisposable();

        Observable
            .Timer(TimeSpan.FromSeconds(_delay))
            .Subscribe(_ =>
            {
                SetInitialOutputValue(OrInputsValue);
                disposable.Dispose();
            })
            .AddTo(disposable);        
    }
}
