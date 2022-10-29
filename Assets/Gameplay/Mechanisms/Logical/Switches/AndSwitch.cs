using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _firstMechanism;
    [SerializeField] private LogicalMechanism _secondMechanism;

    private IReadOnlyReactiveProperty<bool> _output;
    public override IReadOnlyReactiveProperty<bool> Output => _output;

    private void Awake()
    {
        _output = Observable
            .CombineLatest(_firstMechanism.Output, _secondMechanism.Output, (a, b) => a && b)
            .ToReactiveProperty();
    }
}
