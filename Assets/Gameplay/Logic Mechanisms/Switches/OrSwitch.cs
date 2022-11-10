using UnityEngine;
using UniRx;

public class OrSwitch : LogicalMechanism
{
    private void OnValidate()
    {
        if (_inputs == null || _inputs.Length < 2)
        {
            Debug.LogError($"{gameObject} Or switch should have at least 2 inputs");
        }
    }

    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Subscribe(value =>
                {
                    if (value == true)
                    {
                        _output.Value = true;
                    }
                    else
                    {
                        _output.Value = OrInputsValue;
                    }
                })
                .AddTo(Disposable);
        }
    }

    private void Start() => SetInitialOutputValue(OrInputsValue);
}
