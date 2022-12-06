using UnityEngine;
using UniRx;

public class OrSwitch : LogicalMechanism
{
    [SerializeField] private bool _notifyWhenInputChanged;

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
                    SetOutputValue(true);
                }
                else
                {
                    SetOutputValue(OrInputsValue);
                }
            })
            .AddTo(Disposable);
    }

    private void SetOutputValue(bool value)
    {
        if (_notifyWhenInputChanged == true)
        {
            Output.SetValueAndForceNotify(value);
        }
        else
        {
            Output.Value = value;
        }
    }
}
