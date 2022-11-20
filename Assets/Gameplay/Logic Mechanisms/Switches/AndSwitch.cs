using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    private bool AndInputsValue
    {
        get
        {
            foreach (LogicalMechanism input in Inputs)
            {
                if (input.ReadOnlyOutput.Value == false)
                {                    
                    return false;
                }
            }

            return true;
        }
    }

    private void OnValidate()
    {
        if (Inputs.Length < 2)
        {
            Debug.LogError($"{gameObject} And switch should have at least 2 inputs");
        }
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value =>
            {
                if (value == false)
                {
                    Output.Value = false;
                }
                else
                {
                    Output.Value = AndInputsValue;
                }
            })
            .AddTo(Disposable);
    }
}
