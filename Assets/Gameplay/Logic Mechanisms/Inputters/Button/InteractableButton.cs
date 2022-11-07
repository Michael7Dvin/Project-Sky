using System.Collections;
using UnityEngine;

public class InteractableButton : LogicalMechanism, IInteractable
{
    [SerializeField] private bool _isInteractionAllowed = true;

    [Range(0, float.MaxValue)]
    [SerializeField] private float _stickingTime;
    private WaitForSeconds _waitStickingTime;

    public bool IsInteractionAllowed => _isInteractionAllowed;

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
