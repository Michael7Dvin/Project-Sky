using UniRx;

public class Invertor : LogicalMechanism
{
    protected override void SubscribeOnInput(IReadOnlyReactiveProperty<bool> input)
    {
        input
            .Skip(1)
            .Subscribe(value => Output.SetValueAndForceNotify(!value))
            .AddTo(Disposable);
    }
}
