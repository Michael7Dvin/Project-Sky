using System;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public abstract class LogicalMechanism : MonoBehaviour
{
    protected const float INITIAL_VALUE_SET_DELAY = 0.01f;

    [SerializeField] private bool _initialValue;
    [SerializeField] private bool _fireInitialValueToSubscribers;

    [SerializeField] private LogicalMechanism[] _inputs;

    private ReactiveProperty<bool> _output;

    public IReadOnlyReactiveProperty<bool> ReadOnlyOutput
    {
        get
        {
            if (_output == null)
            {
                _output = new ReactiveProperty<bool>(_initialValue);
            }

            return _output;
        }
    }

    protected ReactiveProperty<bool> Output
    {
        get
        {
            if (_output == null)
            {
                _output = new ReactiveProperty<bool>(_initialValue);
            }

            return _output;
        }
    }        

    protected LogicalMechanism[] Inputs => _inputs;
    protected CompositeDisposable Disposable { get; private set; } = new CompositeDisposable();

    protected virtual void Awake()
    {
        FireInitialValue(INITIAL_VALUE_SET_DELAY);
    }

    protected virtual void OnEnable()
    {
        foreach (LogicalMechanism input in Inputs)
        {
            SubscribeOnInput(input.Output);
        }
    }

    private void OnDisable() => Disposable.Clear();

    protected async void FireInitialValue(float delay)
    {
        if (_fireInitialValueToSubscribers == true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Output.SetValueAndForceNotify(_initialValue);
        }
    }

    protected abstract void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input);
}
