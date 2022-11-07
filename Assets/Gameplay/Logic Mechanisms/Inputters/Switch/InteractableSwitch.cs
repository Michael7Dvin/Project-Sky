using UnityEngine;

public class InteractableSwitch : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;
    [SerializeField] private bool _initalValue;

    public bool IsInteractionAllowed => _isInteractionAllowed;

    private void Awake() => _output.Value = _initalValue;

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
