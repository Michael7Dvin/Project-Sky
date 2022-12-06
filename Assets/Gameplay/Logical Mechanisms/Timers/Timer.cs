using System;
using UnityEngine;
using UniRx;

public class Timer : LogicalMechanism
{    
    [Range(0, float.MaxValue)]
    [SerializeField] private float _time;

    protected override void OnEnable()
    {
        base.OnEnable();

        ReadOnlyOutput
            .Where(value => value == true)
            .Delay(TimeSpan.FromSeconds(_time))
            .Subscribe(value =>
            {
                Output.Value = false;
            })
            .AddTo(Disposable);
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
           .Skip(1)
           .Where(value => value == true)
           .Subscribe(value =>
           {
               Output.Value = true;
           })
           .AddTo(Disposable);
    }
}
