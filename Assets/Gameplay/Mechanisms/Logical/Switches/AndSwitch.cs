using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _firstMechanism;
    [SerializeField] private LogicalMechanism _secondMechanism;

    private void Awake()
    {
        if (_firstMechanism == null || _secondMechanism == null)
        {
            Debug.LogError($"{gameObject} And switch should have 2 input logical mechanisms");
        }
    }

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
