using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _firstMechanism;
    [SerializeField] private LogicalMechanism _secondMechanism;

    public override IReadOnlyReactiveProperty<bool> Output
    {
        get
        {
            return Observable
                .CombineLatest(_firstMechanism.Output, _secondMechanism.Output, (a, b) => a && b)
                .ToReactiveProperty();
        }
    }
}
