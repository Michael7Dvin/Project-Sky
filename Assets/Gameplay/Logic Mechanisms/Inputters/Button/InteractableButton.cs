using System;
using UnityEngine;
using UniRx;

public class InteractableButton : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _stickingTime;

    public bool IsInteractionAllowed => _isInteractionAllowed;

    protected override void OnEnable()
    {
        base.OnEnable();

        Output
            .Where(value => value == true)
            .Delay(TimeSpan.FromSeconds(_stickingTime))
            .Subscribe(value => Output.Value = false)
            .AddTo(Disposable);
    }

    public void Interact()
    {
        if (ReadOnlyOutput.Value == false)
        {
            Output.Value = true;
        }        
    }

    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Where(value => value == true)
            .Subscribe(value => Output.Value = value)
            .AddTo(Disposable);
    }
}
