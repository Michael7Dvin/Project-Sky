using UnityEngine;
using UniRx;

public class InteractableSwitch : LogicalMechanism, IInteractable
{
    protected readonly ReactiveProperty<bool> _status = new ReactiveProperty<bool>();

    [SerializeField] private bool _isInteractionAllowed = true;

    [SerializeField] private bool _initalStatus;

    public bool IsInteractionAllowed => _isInteractionAllowed;
    public override IReadOnlyReactiveProperty<bool> Output => _status;

    private void Awake() => _status.Value = _initalStatus;

    public void Interact()
    {
        if (_isInteractionAllowed == true)
        {
            if (_status.Value == true)
            {
                _status.Value = false;
            }
            else
            {
                _status.Value = true;
            }
        }
    }
}
