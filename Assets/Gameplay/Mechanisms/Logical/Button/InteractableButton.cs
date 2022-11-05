using System.Collections;
using UnityEngine;
using UniRx;

public class InteractableButton : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _stickingTime;
    private WaitForSeconds _waitStickingTime;

    private readonly ReactiveProperty<bool> _output = new ReactiveProperty<bool>();

    public bool IsInteractionAllowed => _isInteractionAllowed;
    public override IReadOnlyReactiveProperty<bool> Output => _output;

    private void Awake()
    {
        _waitStickingTime = new WaitForSeconds(_stickingTime);
    }

    public void Interact()
    {
        if (_output.Value == false)
        {
            StartCoroutine(Press());
        }        
    }

    private IEnumerator Press()
    {
        _output.Value = true;

        yield return _waitStickingTime;
        
        _output.Value = false;
    }
}
