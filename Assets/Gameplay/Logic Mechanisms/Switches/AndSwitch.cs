using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    private bool AndInputsValue
    {
        get
        {
            foreach (LogicalMechanism input in _inputs)
            {
                if (input.Output.Value == false)
                {                    
                    return false;
                }
            }

            return true;
        }
    }

    private void OnValidate()
    {
        if (_inputs == null || _inputs.Length < 2)
        {
            Debug.LogError($"{gameObject} And switch should have at least 2 inputs");
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
                    if (value == false)
                    {
                        _output.Value = false;
                    }
                    else
                    {
                        _output.Value = AndInputsValue;
                    }
                })
                .AddTo(Disposable);
        }
    }

    private void Start() => SetInitialOutputValue(AndInputsValue);
}
