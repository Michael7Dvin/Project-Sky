using System;
using UnityEngine;
using UniRx;

public class Timer : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _input;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _time;

    private void OnEnable()
    {
        _input
            .Output
            .Where(value => value == true)
            .Subscribe(value =>
            {
                _output.Value = true;
            })
            .AddTo(_disposable);

        _input
            .Output
            .Where(value => value == true)
            .Delay(TimeSpan.FromSeconds(_time))
            .Subscribe(value =>
            {
                _output.Value = false;
            })
            .AddTo(_disposable);
    }
}
