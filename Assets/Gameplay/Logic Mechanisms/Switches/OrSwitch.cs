using UnityEngine;
using UniRx;

public class OrSwitch : LogicalMechanism
{
    private bool OrInputsValue
    {
        get
        {
            foreach (LogicalMechanism input in Inputs)
            {
                if (input.ReadOnlyOutput.Value == true)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void OnValidate()
    {
        if (Inputs.Length < 2)
        {
            Debug.LogError($"{gameObject} Or switch should have at least 2 inputs");
        }
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value =>
            {
                if (value == true)
                {
                    Output.Value = true;
                }
                else
                {
                    Output.Value = OrInputsValue;
                }
            })
            .AddTo(Disposable);
    }
}
