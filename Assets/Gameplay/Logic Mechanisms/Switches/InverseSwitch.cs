using UnityEngine;
using UniRx;

public class InverseSwitch : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _input;

    private void OnEnable()
    {
        _input
            .Output
            .Subscribe(value => _output.Value = !value)
            .AddTo(_disposable);
    }
}
