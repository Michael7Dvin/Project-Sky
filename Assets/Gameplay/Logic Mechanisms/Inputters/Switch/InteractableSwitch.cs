using UnityEngine;
using UniRx;

public class InteractableSwitch : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    public bool IsInteractionAllowed => _isInteractionAllowed;


    private void OnEnable()
    {                
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Subscribe(value => _output.Value = value)
                .AddTo(Disposable);
        }
    }

    private void Start() => SetInitialOutputValue(OrInputsValue);

    public void Interact()
    {
        if (_isInteractionAllowed == true)
        {
            _output.Value = !_output.Value;
        }
    }
}
