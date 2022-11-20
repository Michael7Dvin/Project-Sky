using UniRx;

public class InverseSwitch : LogicalMechanism
{
    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value => Output.SetValueAndForceNotify(!value))
            .AddTo(Disposable);
    }
}
