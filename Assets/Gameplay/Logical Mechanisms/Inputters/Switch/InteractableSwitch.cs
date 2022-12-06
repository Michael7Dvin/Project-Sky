using UnityEngine;
using UniRx;

public class InteractableSwitch : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    public bool IsInteractionAllowed => _isInteractionAllowed;

    public void Interact()
    {
        if (_isInteractionAllowed == true)
        {
            Output.Value = !Output.Value;
        }
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value => Output.Value = value)
            .AddTo(Disposable);
    }
}
