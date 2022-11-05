using UnityEngine;
using UniRx;

public class InteractableSwitch : LogicalMechanism, IInteractable
{
    protected readonly ReactiveProperty<bool> _output = new ReactiveProperty<bool>();

    [SerializeField] private bool _isInteractionAllowed = true;

    [SerializeField] private bool _initalStatus;

    public bool IsInteractionAllowed => _isInteractionAllowed;
    public override IReadOnlyReactiveProperty<bool> Output => _output;

    private void Awake() => _output.Value = _initalStatus;

    public void Interact()
    {
        if (_isInteractionAllowed == true)
        {
            if (_output.Value == true)
            {
                _output.Value = false;
            }
            else
            {
                _output.Value = true;
            }
        }
    }
}
