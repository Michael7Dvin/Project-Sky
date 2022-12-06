using System;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class Delay : LogicalMechanism
{
    [Range(0, float.MaxValue)]
    [SerializeField] private float _delay;

    protected override void Awake()
    {
        FireInitialValue(_delay);
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Delay(TimeSpan.FromSeconds(_delay))
            .Subscribe(value =>
            {
                Output.SetValueAndForceNotify(value);
            })
            .AddTo(Disposable);
    }
}
