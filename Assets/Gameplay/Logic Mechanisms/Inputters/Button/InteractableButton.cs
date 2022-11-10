using System;
using UnityEngine;
using UniRx;

public class InteractableButton : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _stickingTime;

    public bool IsInteractionAllowed => _isInteractionAllowed;
 
    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
                .Output
                .Skip(1)
                .Where(value => value == true)
                .Subscribe(value => _output.Value = value)
                .AddTo(Disposable);
        }

        Output
            .Where(value => value == true)
            .Delay(TimeSpan.FromSeconds(_stickingTime))
            .Subscribe(value =>
            {
                _output.Value = false;
            })
            .AddTo(Disposable);
    }

    private void Start() => SetInitialOutputValue(OrInputsValue);

    public void Interact()
    {
        if (_output.Value == false)
        {
            Press();
        }        
    }

    private void Press()
    {
        _output.Value = true;
    }
}
