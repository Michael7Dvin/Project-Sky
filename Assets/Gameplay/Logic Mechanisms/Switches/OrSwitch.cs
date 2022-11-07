using UnityEngine;
using UniRx;

public class OrSwitch : LogicalMechanism
{
    [SerializeField] private LogicalMechanism _firstInput;
    [SerializeField] private LogicalMechanism _secondInput;

    private void Awake()
    {
        if (_firstInput == null || _secondInput == null)
        {
            Debug.LogError($"{gameObject} Or switch should have 2 input logical mechanisms");
        }
    }

    private void OnEnable()
    {
        _firstInput
            .Output
            .Subscribe(value => OnInputChanged())
            .AddTo(_disposable);
        
        _secondInput
            .Output
            .Subscribe(value => OnInputChanged())
            .AddTo(_disposable);

        void OnInputChanged()
        {
            if (_firstInput.Output.Value == true || _secondInput.Output.Value == true)
            {
                _output.Value = true;
            }
            else
            {
                _output.Value = false; 
            }
        }
    }
}
