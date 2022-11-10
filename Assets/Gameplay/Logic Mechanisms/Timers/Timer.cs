using System;
using UnityEngine;
using UniRx;

public class Timer : LogicalMechanism
{    
    [Range(0, float.MaxValue)]
    [SerializeField] private float _time;

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Where(value => value == true)
                .Subscribe(value =>
                {
                    _output.Value = value;
                })
                .AddTo(Disposable);        
        }
        

        Output
            .Where(value => value == true)
            .Delay(TimeSpan.FromSeconds(_time))
            .Subscribe(value =>
            {
                _output.Value = false;
            })
            .AddTo(Disposable);
    }

    private void Start() => SetInitialOutputValue(OrInputsValue);
}
