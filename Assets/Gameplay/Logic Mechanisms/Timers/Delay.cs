using System;
using UnityEngine;
using UniRx;

public class Delay : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _input;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;

    private void OnEnable()
    {
        _input
            .Output
            .Delay(TimeSpan.FromSeconds(_delay))
            .Subscribe(value => _output.Value = value)
            .AddTo(_disposable);
    }
}
