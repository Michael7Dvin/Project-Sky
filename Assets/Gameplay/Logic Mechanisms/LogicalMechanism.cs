using UnityEngine;
using UniRx;

public abstract class LogicalMechanism : MonoBehaviour
{
    [SerializeField] protected LogicalMechanism[] _inputs;
    protected readonly ReactiveProperty<bool> _output = new ReactiveProperty<bool>();
    
    [SerializeField] private bool _initalOutputValue;
    private bool _isInitialValueWasSet;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    
    public IReadOnlyReactiveProperty<bool> Output => _output;
    protected bool OrInputsValue
    {
        get
        {
            if (_inputs == null)
            {
                return false;
            }

            foreach (LogicalMechanism input in _inputs)
            {
                if (input.Output.Value == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
    protected CompositeDisposable Disposable => _disposable;
  
    private void OnDisable() => Disposable.Clear();

    protected void SetInitialOutputValue(bool inputsValue)
    {
        if (_isInitialValueWasSet == false)
        {
            _output.Value = _initalOutputValue || inputsValue;
            _isInitialValueWasSet = true;
        }        
    }
}
