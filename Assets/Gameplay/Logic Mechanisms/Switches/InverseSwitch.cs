using UniRx;

public class InverseSwitch : LogicalMechanism
{
    private void OnEnable()
    {
        foreach (LogicalMechanism input in _inputs)
        {
            input
               .Output
               .Skip(1)
               .Subscribe(value => _output.Value = !value)
               .AddTo(Disposable);
        }          
    }

    private void Start() => SetInitialOutputValue(!OrInputsValue);
}
