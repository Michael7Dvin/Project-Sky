using UnityEngine;
using UniRx;

public class AndSwitch : LogicalMechanism
{
    [SerializeField] private bool _notifyWhenInputChanged;

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
                    SetOutputValue(false);
                }
                else
                {
                    SetOutputValue(AndInputsValue);
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
